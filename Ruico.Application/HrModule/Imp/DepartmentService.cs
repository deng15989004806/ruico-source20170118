using System;
using System.Linq;
using System.Text;
using PagedList;
using Ruico.Application.Exceptions;
using Ruico.Application.Extensions;
using Ruico.Application.Resources.Generated;
using Ruico.Application.SystemModule.Imp;
using Ruico.Domain.HrModule.Entities;
using Ruico.Domain.HrModule.Repositories;
using Ruico.Domain.Weixin.Service;
using Ruico.Dto.Hr;
using Ruico.Dto.Hr.Converters;
using Ruico.Infrastructure.Entity;
using Ruico.Infrastructure.Utility.Helper;

namespace Ruico.Application.HrModule.Imp
{
    public class DepartmentService : IDepartmentService
    {
        IDepartmentRepository _Repository;
        IContactsService _contactsService;
        ICommonService _commonService;

        #region Constructors

        public DepartmentService(IDepartmentRepository repository,
            IContactsService contactsService,
            ICommonService commonService)                               
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            _Repository = repository;
            _contactsService = contactsService;
            _commonService = commonService;
        }

        #endregion

        private void ValidateModel(Department model)
        {
            if (model.Name.IsNullOrBlank())
            {
                throw new DefinedException(CommonMessageResources.Name_Empty);
            }

            if (model.DepartmentId <= 0)
            {
                throw new DefinedException(HrMessagesResources.DepartmentId_Empty);
            }

            if (model.ParentId <= 0)
            {
                throw new DefinedException(HrMessagesResources.DepartmentParentId_Empty);
            }

            if (_Repository.Exists(model))
            {
                throw new DataExistsException(string.Format(HrMessagesResources.Department_Exists_WithValue, model.Name));
            }
        }

        private void OperationLog(string action, DepartmentDTO itemDto, DepartmentDTO oldDto = null)
        {
            OperateRecorder.RecordOperation(itemDto.Id.ToString(), action,
                itemDto.GetOperationLog(oldDto));
        }

        public DepartmentDTO Add(DepartmentDTO itemDto)
        {
            var model = itemDto.ToModel();
            model.Id = IdentityGenerator.NewSequentialGuid();
            model.Created = DateTime.UtcNow;

            // 数据验证
            this.ValidateModel(model);
            _Repository.Add(model);

            this.OperationLog(HrMessagesResources.Add_Department, model.ToDto(),  null);

            //commit the unit of work
            _Repository.UnitOfWork.Commit();

            return model.ToDto();
        }

        public void Update(DepartmentDTO itemDto)
        {
            //get persisted item
            var persistedModel = _Repository.Get(itemDto.Id);

            if (persistedModel == null)
            {
                throw new DataNotFoundException(HrMessagesResources.Department_NotExists);
            }

            var oldDTO = persistedModel.ToDto();

            // 可以修改的字段
            var current = oldDTO.ToModel();
            current.Name = itemDto.Name;
            current.DepartmentId = itemDto.DepartmentId;
            current.SortOrder = itemDto.SortOrder;
            current.ParentId = itemDto.ParentId;

            // 数据验证
            this.ValidateModel(persistedModel);

            this.OperationLog(HrMessagesResources.Update_Department, persistedModel.ToDto(), oldDTO);

            //Merge changes
            _Repository.Merge(persistedModel, current);
            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public DepartmentDTO Get(int departmentId)
        {
            var persistedModel = _Repository.Find(x=>x.DepartmentId == departmentId);

            if (persistedModel == null)
            {
                return null;
            }

            return persistedModel.ToDto();
        }

        public void Remove(Guid id)
        {
            var persistedModel = _Repository.Get(id);

            if (persistedModel == null)
            {
                throw new DataNotFoundException(HrMessagesResources.Department_NotExists);
            }

            var departmentDto = persistedModel.ToDto();

            _Repository.Remove(persistedModel);

            this.OperationLog(HrMessagesResources.Add_Department, departmentDto, null);

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public DepartmentDTO FindBy(Guid id)
        {
            var persistedModel = _Repository.Get(id);

            if (persistedModel == null)
            {
                throw new DataNotFoundException(HrMessagesResources.Department_NotExists);
            }

            return persistedModel.ToDto();
        }

        public IPagedList<DepartmentDTO> FindBy(int? appId, string name, int pageNumber, int pageSize)
        {
            var list = _Repository.FindBy(appId, name, pageNumber, pageSize);

            var result = list.ToList();

            return new StaticPagedList<DepartmentDTO>(
                result.Select(x => x.ToDto()),
                pageNumber,
                pageSize,
                list.TotalItemCount);
        }

        public void DownloadDepartments()
        {
            var accessToken = _commonService.GetContactsAccessToken();
            var departments = _contactsService.GetDepartments(accessToken);

            var sbError = new StringBuilder();
            foreach (var dep in departments)
            {
                if (dep.Id == 1)
                {
                    continue;
                }

                if (dep.Id > 0)
                {
                    try
                    {
                        var dto = new DepartmentDTO()
                        {
                            Name = dep.Name,
                            DepartmentId = dep.Id,
                            SortOrder = dep.Order,
                            ParentId = dep.ParentId
                        };
                        var model = _Repository.Find(x => x.DepartmentId == dep.Id);
                        if (model == null)
                        {
                            this.Add(dto);
                        }
                        else
                        {
                            dto.Id = model.Id;
                            dto.Created = model.Created;
                            this.Update(dto);
                        }
                    }
                    catch (Exception ex)
                    {
                        sbError.AppendLine(string.Format("{0} {1}", dep.Name, ex.Message));
                    }
                }
            }

            if (sbError.Length > 0)
            {
                throw new Exception(sbError.ToString());
            }
        }

        public void UploadDepartments()
        {
            var list = _Repository.FindBy(null, null, 1, int.MaxValue);

            var accessToken = _commonService.GetContactsAccessToken();
            var departments = _contactsService.GetDepartments(accessToken);

            var sbError = new StringBuilder();

            foreach (var dep in list)
            {
                if (dep.DepartmentId == 1)
                {
                    continue;
                }

                try
                {
                    var item = departments.FirstOrDefault(x => x.Id == dep.DepartmentId);
                    var model = new Domain.Weixin.Model.Department(dep.DepartmentId, dep.Name,dep.ParentId, dep.SortOrder);

                    if (item != null)
                    {
                        _contactsService.UpdateDepartment(accessToken, model);
                    }
                    else
                    {
                        _contactsService.CreateDepartment(accessToken, model);
                    }
                }
                catch (Exception ex)
                {
                    sbError.AppendLine(string.Format("{0} {1}", dep.Name, ex.Message));
                }
            }

            if (sbError.Length > 0)
            {
                throw new Exception(sbError.ToString());
            }
        }

        public void RemoveNotExistDepartmentInWeixin()
        {
            var list = _Repository.FindBy(null, null, 1, int.MaxValue);

            var accessToken = _commonService.GetContactsAccessToken();
            var departments = _contactsService.GetDepartments(accessToken);

            var sbError = new StringBuilder();

            var deletedList = departments.Where(x => !list.Select(d => d.DepartmentId).Contains(x.Id));

            foreach (var dep in deletedList)
            {
                if (dep.Id == 1)
                {
                    continue;
                }

                try
                {
                    _contactsService.DeleteDepartment(accessToken, dep.Id);
                }
                catch (Exception ex)
                {
                    sbError.AppendLine(string.Format("{0} {1}", dep.Name, ex.Message));
                }
            }

            if (sbError.Length > 0)
            {
                throw new Exception(sbError.ToString());
            }
        }
    }
}
