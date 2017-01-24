using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using Ruico.Application.Exceptions;
using Ruico.Application.Extensions;
using Ruico.Application.Resources.Generated;
using Ruico.Application.SystemModule.Imp;
using Ruico.Domain.Weixin.Model;
using Ruico.Domain.Weixin.Service;
using Ruico.Domain.WeixinModule.Entities;
using Ruico.Domain.WeixinModule.Repositories;
using Ruico.Dto.Common;
using Ruico.Dto.Weixin;
using Ruico.Dto.Weixin.Converters;
using Ruico.Infrastructure.Entity;
using Ruico.Infrastructure.Utility.Helper;

namespace Ruico.Application.WeixinModule.Imp
{
    public class AppMenuService : IAppMenuService
    {
        IAppMenuRepository _Repository;
        ICommonService _CommonService;
        IMenuService _MenuService;

        #region Constructors

        public AppMenuService(IAppMenuRepository repository,
            ICommonService commonService,
            IMenuService menuService)                               
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            _Repository = repository;
            _CommonService = commonService;
            _MenuService = menuService;
        }

        #endregion

        private void ValidateModel(AppMenu model)
        {
            if (model.Name.IsNullOrBlank())
            {
                throw new DefinedException(CommonMessageResources.Name_Empty);
            }

            if (_Repository.Exists(model))
            {
                throw new DataExistsException(string.Format(WeixinMessagesResources.AppMenu_Exists_WithValue, model.Name));
            }
        }

        private void OperationLog(string action, AppMenuDTO itemDto, AppMenuDTO oldDto = null)
        {
            OperateRecorder.RecordOperation(itemDto.Id.ToString(), action,
                itemDto.GetOperationLog(oldDto));
        }

        public AppMenuDTO Add(AppMenuDTO itemDto)
        {
            var model = itemDto.ToModel();
            model.Id = IdentityGenerator.NewSequentialGuid();
            model.Created = DateTime.UtcNow;

            if (model.Parent != null && model.Parent.Id != Guid.Empty)
            {
                model.Parent = _Repository.Get(itemDto.Parent.Id);
            }

            // 数据验证
            this.ValidateModel(model);
            _Repository.Add(model);

            this.OperationLog(WeixinMessagesResources.Add_AppMenu, model.ToDto(),  null);

            //commit the unit of work
            _Repository.UnitOfWork.Commit();

            return model.ToDto();
        }

        public void Update(AppMenuDTO itemDto)
        {
            //get persisted item
            var persistedModel = _Repository.Get(itemDto.Id);

            if (persistedModel == null)
            {
                throw new DataNotFoundException(WeixinMessagesResources.AppMenu_NotExists);
            }

            var oldDTO = persistedModel.ToDto();

            // 可以修改的字段
            var current = oldDTO.ToModel();
            current.Name = itemDto.Name;
            current.Key = itemDto.Key;
            current.Url = itemDto.Url;
            current.SortOrder = itemDto.SortOrder;
            current.AppId = itemDto.AppId;
            if (itemDto.Parent != null)
            {
                current.Parent = _Repository.Get(itemDto.Parent.Id);
            }

            // 数据验证
            this.ValidateModel(persistedModel);

            this.OperationLog(WeixinMessagesResources.Update_AppMenu, persistedModel.ToDto(), oldDTO);

            // Merge changes
            _Repository.Merge(persistedModel, current);
            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public void Remove(Guid id)
        {
            var persistedModel = _Repository.Get(id);

            if (persistedModel == null)
            {
                throw new DataNotFoundException(WeixinMessagesResources.AppMenu_NotExists);
            }

            var menuDto = persistedModel.ToDto();

            _Repository.Remove(persistedModel);

            this.OperationLog(WeixinMessagesResources.Add_AppMenu, menuDto, null);

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public AppMenuDTO FindBy(Guid id)
        {
            var persistedModel = _Repository.Get(id);

            if (persistedModel == null)
            {
                throw new DataNotFoundException(WeixinMessagesResources.AppMenu_NotExists);
            }

            return persistedModel.ToDto();
        }

        public IPagedList<AppMenuDTO> FindBy(int? appId, string name, int pageNumber, int pageSize)
        {
            var list = _Repository.FindBy(appId, name, pageNumber, pageSize);

            var result = list.ToList();

            return new StaticPagedList<AppMenuDTO>(
                result.Select(x => x.ToDto()),
                pageNumber,
                pageSize,
                list.TotalItemCount);
        }

        public IList<NameValueDTO> GetApps()
        {
            return _CommonService.GetApps().Select(x => new NameValueDTO()
            {
                Name = x.Name,
                Value = x.Value,
                Key = x.Key,
            }).ToList();
        }

        public void DownloadMenus(int appId)
        {
            var apps = _CommonService.GetApps();
            var app = apps.FirstOrDefault(x => x.Value == appId.ToString());

            if (app == null)
            {
                throw new Exception("app not found. appId:" + appId);
            }

            var accessToken = _CommonService.GetAccessToken(app.Key);
            var menus = _MenuService.GetMenus(accessToken, appId.ToString());

            // 删除旧的菜单
            var list = _Repository.FindBy(appId, null, 1, int.MaxValue);
            foreach (var m in list)
            {
                _Repository.Remove(m);
            }
            
            // 添加新的菜单
            var parentSortOrder = 1;
            foreach (var m in menus)
            {
                var parent = new AppMenu();
                parent.Id = IdentityGenerator.NewSequentialGuid();
                parent.Created = DateTime.UtcNow;
                parent.Name = m.Menu.Name;
                parent.SortOrder = parentSortOrder++;
                parent.AppId = appId;
                _Repository.Add(parent);

                var childSortOrder = 10;
                foreach (var c in m.Childs)
                {
                    var child = new AppMenu();
                    child.Id = IdentityGenerator.NewSequentialGuid();
                    child.Created = DateTime.UtcNow;
                    child.Name = c.Name;
                    child.Key = c.Key;
                    child.Url = c.Url;
                    child.SortOrder = childSortOrder++;
                    child.AppId = appId;
                    child.Parent = parent;
                    _Repository.Add(child);
                }
            }

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public void UploadMenus(int appId)
        {
            var apps = _CommonService.GetApps();
            var app = apps.FirstOrDefault(x => x.Value == appId.ToString());

            if (app == null)
            {
                throw new Exception("app not found. appId:" + appId);
            }

            var accessToken = _CommonService.GetAccessToken(app.Key);
            
            var list = _Repository.FindBy(appId, null, 1, int.MaxValue);

            var menuGroups = new List<MenuGroup>();
            if (list.Any())
            {
                menuGroups = list.Where(x => x.Parent == null)
                    .Select(
                        x =>
                            new MenuGroup()
                            {
                                Menu = new Menu(x.Name),
                                Childs = list.Where(c => c.Parent != null && c.Parent.Id == x.Id)
                                    .Select(c => new Menu(string.IsNullOrWhiteSpace(c.Url) ? "click" : "view",              c.Name, c.Key, c.Url)).ToList()
                            }).ToList();
            }

            _MenuService.Create(accessToken, appId.ToString(), menuGroups);
        }
    }
}
