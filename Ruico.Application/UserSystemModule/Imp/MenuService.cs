using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PagedList;
using Ruico.Application.Exceptions;
using Ruico.Application.Extensions;
using Ruico.Application.Resources.Generated;
using Ruico.Application.SystemModule.Imp;
using Ruico.Domain.UserSystemModule.Entities;
using Ruico.Domain.UserSystemModule.Repositories;
using Ruico.Dto.UserSystem;
using Ruico.Dto.UserSystem.Converters;
using Ruico.Infrastructure.Entity;
using Ruico.Infrastructure.Utility.Extensions;
using Ruico.Infrastructure.Utility.Helper;

namespace Ruico.Application.UserSystemModule.Imp
{
    public class MenuService : IMenuService
    {
        IMenuRepository _Repository;
        IPermissionRepository _PermissionRepository;
        IModuleRepository _ModuleRepository;

        #region Constructors

        public MenuService(IMenuRepository repository, 
            IPermissionRepository permissionRepository,
            IModuleRepository moduleRepository)                               
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            _Repository = repository;
            _PermissionRepository = permissionRepository;
            _ModuleRepository = moduleRepository;
        }

        #endregion

        public MenuDTO Add(MenuDTO menuDTO)
        {
            var menu = menuDTO.ToModel();
            menu.Id = IdentityGenerator.NewSequentialGuid();
            menu.Created = DateTime.UtcNow;
            if (menuDTO.Module != null)
            {
                menu.Module = _ModuleRepository.Get(menuDTO.Module.Id);
            }
            if (menu.Parent == null || menu.Parent.Id == Guid.Empty)
            {
                menu.Depth = 1;
            }
            else
            {
                menu.Parent = _Repository.Get(menuDTO.Parent.Id);
                menu.Depth = menu.Parent.Depth + 1;
            }

            if (menu.Name.IsNullOrBlank())
            {
                throw new DefinedException(UserSystemMessagesResources.Name_Empty);
            }

            if (menu.Module == null || menu.Module.Id == Guid.Empty)
            {
                throw new DefinedException(UserSystemMessagesResources.Module_Empty);
            }

            if (_Repository.Exists(menu))
            {
                throw new DataExistsException(string.Format(UserSystemMessagesResources.Menu_Exists_WithValue, menu.Name));
            }

            foreach (var p in menu.Permissions)
            {
                if (p.Id == Guid.Empty)
                {
                    p.Id = IdentityGenerator.NewSequentialGuid();
                    p.Created = DateTime.UtcNow;
                }
            }

            menu.Module = _ModuleRepository.Get(menu.Module.Id);

            _Repository.Add(menu);

            #region 操作日志

            var menuDto = menu.ToDto();

            OperateRecorder.RecordOperation(menuDto.Id.ToString(),
                UserSystemMessagesResources.Add_Menu,
                menuDto.GetOperationLog());

            #endregion

            //commit the unit of work
            _Repository.UnitOfWork.Commit();

            return menu.ToDto();
        }

        public void Update(MenuDTO menuDTO)
        {
            //get persisted item
            var menu = _Repository.Get(menuDTO.Id);

            if (menu == null)
            {
                throw new DataNotFoundException(UserSystemMessagesResources.Menu_NotExists);
            }

            var oldDTO = menu.ToDto();

            var current = menuDTO.ToModel();

            menu.Name = current.Name;
            menu.Module = _ModuleRepository.Get(menuDTO.Module.Id);
            if (current.Parent == null || current.Parent.Id == Guid.Empty)
            {
                menu.Depth = 1;
            }
            else
            {
                menu.Parent = _Repository.Get(current.Parent.Id);
                menu.Depth = current.Parent.Depth + 1;
            }
            menu.SortOrder = current.SortOrder;
            menu.Url = current.Url;
            menu.Code = current.Code;


            if (menu.Name.IsNullOrBlank())
            {
                throw new DefinedException(UserSystemMessagesResources.Name_Empty);
            }
            if (current.Module == null || current.Module.Id == Guid.Empty)
            {
                throw new DefinedException(UserSystemMessagesResources.Module_Empty);
            }

            if (_Repository.Exists(menu))
            {
                throw new DataExistsException(string.Format(UserSystemMessagesResources.Menu_Exists_WithValue, menu.Name));
            }
            menu.Module = _ModuleRepository.Get(menu.Module.Id);

            if (menu.Module.Id != oldDTO.Module.Id)
            {
                // 如果更换了模块
                var oldModuleMenus = _Repository.FindBy(oldDTO.Module.Id, string.Empty, 1, int.MaxValue);
                var allChildMenus = new List<Menu>();
                GetAllChildMenus(menu.Id, oldModuleMenus, ref allChildMenus);
                if (allChildMenus.Any())
                {
                    // 所有子菜单都更换为新的Module
                    foreach (var m in allChildMenus)
                    {
                        m.Module = menu.Module;

                        #region 操作日志

                        var mDto = m.ToDto();

                        OperateRecorder.RecordOperation(mDto.Id.ToString(),
                            UserSystemMessagesResources.Update_Menu,
                            UserSystemMessagesResources.Menu_UpdateChildMenuModule + ":" +
                            string.Format("{0} => {1}", oldDTO.Module.Name, menu.Module.Name));

                        #endregion
                    }
                }
            }

            #region 操作日志

            var menuDto = menu.ToDto();

            OperateRecorder.RecordOperation(menuDto.Id.ToString(),
                UserSystemMessagesResources.Update_Menu,
                menuDto.GetOperationLog(oldDTO));

            #endregion

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        private void GetAllChildMenus(Guid menuId, IEnumerable<Menu> moduleMenus, ref List<Menu> childMenus)
        {
            var childs = moduleMenus.Where(x => x.Parent != null && x.Parent.Id == menuId).ToList();
            if (childs.Any())
            {
                childMenus.AddRange(childs.ToList());
                foreach (var child in childs)
                {
                    GetAllChildMenus(child.Id, moduleMenus, ref childMenus);
                }
            }
        }

        public void Remove(Guid id)
        {
            var menu = _Repository.Get(id);

            if (menu == null)
            {
                throw new DataNotFoundException(UserSystemMessagesResources.Menu_NotExists);
            }

            var menuDto = menu.ToDto();

            _Repository.Remove(menu);

            #region 操作日志

            OperateRecorder.RecordOperation(menuDto.Id.ToString(),
                UserSystemMessagesResources.Remove_Menu,
                menuDto.GetOperationLog());

            #endregion

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public void UpdatePermission(MenuDTO menuDTO)
        {
            var menu = _Repository.Get(menuDTO.Id);

            if (menu == null)
            {
                throw new DataNotFoundException(UserSystemMessagesResources.Menu_NotExists);
            }

            foreach (var p in menuDTO.Permissions)
            {
                if (p.Id == Guid.Empty)
                {
                    p.Id = IdentityGenerator.NewSequentialGuid();
                    p.Created = DateTime.UtcNow;
                }
            }

            var oldPermissions = menu.Permissions.ToList().Copy();
            var newPermissions = menuDTO.Permissions.Select(x => x.ToModel()).ToList();

            foreach (var p in menu.Permissions.ToArray())
            {
                // 先删除从表数据
                _PermissionRepository.Remove(p);
            }
            menu.Permissions = new Collection<Permission>();
            foreach (var p in newPermissions)
            {
                p.Menu = menu;
                menu.Permissions.Add(p);
            }

            #region 操作日志

            var addPermissions = newPermissions.Except(oldPermissions).ToList();
            var removePermissions = oldPermissions.Except(newPermissions).ToList();

            if (addPermissions.Any())
            {
                foreach (var p in addPermissions)
                {
                    var dto = p.ToDto();
                    OperateRecorder.RecordOperation(dto.Id.ToString(),
                        UserSystemMessagesResources.Add_Permission,
                        dto.GetOperationLog());
                }
            }
            if (removePermissions.Any())
            {
                foreach (var p in addPermissions)
                {
                    var dto = p.ToDto();
                    OperateRecorder.RecordOperation(dto.Id.ToString(),
                        UserSystemMessagesResources.Remove_Permission,
                        dto.GetOperationLog());
                }
            }
            foreach (var p in newPermissions)
            {
                var dto = p.ToDto();
                var oldP = oldPermissions.FirstOrDefault(x => x.Id == p.Id);
                if (oldP != null)
                {
                    if (oldP.Name != p.Name
                        || oldP.Code != p.Code
                        || oldP.SortOrder != p.SortOrder)
                    {
                        OperateRecorder.RecordOperation(dto.Id.ToString(),
                            UserSystemMessagesResources.Update_Permission,
                            dto.GetOperationLog());
                    }
                }
            }

            #endregion

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        private MenuDTO ToMenuDTO(Menu menu)
        {
            var menuDTO = menu.ToDto();
            menuDTO.Permissions = menuDTO.Permissions.OrderBy(x => x.SortOrder).ToList();

            return menuDTO;
        }

        public MenuDTO FindBy(Guid id)
        {
            return this.ToMenuDTO(_Repository.Get(id));
        }

        public IPagedList<MenuDTO> FindBy(Guid moduleId, string name, int pageNumber, int pageSize)
        {
            var list = _Repository.FindBy(moduleId, name, pageNumber, pageSize);

            var result = list.OrderBy(x => x.Module.Id)
                .ThenBy(x=>x.Depth)
                .ThenBy(x => x.SortOrder).ToList();

            return new StaticPagedList<MenuDTO>(
                result.Select(this.ToMenuDTO),
                pageNumber,
                pageSize,
                list.TotalItemCount);
        }

        public List<MenuDTO> FindByModule(Guid moduleId)
        {
            var list = _Repository.Collection.Where(x => x.Module.Id.Equals(moduleId))
                .OrderBy(x => x.Depth)
                .ThenBy(x => x.SortOrder).ToList();
            return list.Select(this.ToMenuDTO).ToList();
        }
    }
}
