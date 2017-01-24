using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using Ruico.Application.Exceptions;
using Ruico.Application.Extensions;
using Ruico.Application.Resources.Generated;
using Ruico.Application.SystemModule.Imp;
using Ruico.Domain.UserSystemModule.Repositories;
using Ruico.Dto.UserSystem;
using Ruico.Dto.UserSystem.Converters;
using Ruico.Infrastructure.Entity;
using Ruico.Infrastructure.Utility.Helper;

namespace Ruico.Application.UserSystemModule.Imp
{
    public class ModuleService : IModuleService
    {
        IModuleRepository _Repository;

        #region Constructors

        public ModuleService(IModuleRepository repository)                               
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            _Repository = repository;
        }

        #endregion

        public ModuleDTO Add(ModuleDTO moduleDTO)
        {
            var module = moduleDTO.ToModel();
            module.Id = IdentityGenerator.NewSequentialGuid();
            module.Created = DateTime.UtcNow;

            if (module.Name.IsNullOrBlank())
            {
                throw new DefinedException(UserSystemMessagesResources.Name_Empty);
            }

            if (_Repository.Exists(module))
            {
                throw new DataExistsException(string.Format(UserSystemMessagesResources.Module_Exists_WithValue, module.Name));
            }

            _Repository.Add(module);

            #region 操作日志

            var moduleDto = module.ToDto();

            OperateRecorder.RecordOperation(moduleDto.Id.ToString(),
                UserSystemMessagesResources.Add_Module,
                moduleDto.GetOperationLog());

            #endregion

            //commit the unit of work
            _Repository.UnitOfWork.Commit();

            return module.ToDto();
        }

        public void Update(ModuleDTO moduleDTO)
        {
            //get persisted item
            var module = _Repository.Get(moduleDTO.Id);

            if (module == null)
            {
                throw new DataNotFoundException(UserSystemMessagesResources.Module_NotExists);
            }

            var oldDTO = module.ToDto();

            var current = moduleDTO.ToModel();

            module.Name = current.Name;
            module.SortOrder = current.SortOrder;

            if (module.Name.IsNullOrBlank())
            {
                throw new DefinedException(UserSystemMessagesResources.Name_Empty);
            }

            if (_Repository.Exists(module))
            {
                throw new DataExistsException(string.Format(UserSystemMessagesResources.Module_Exists_WithValue, module.Name));
            }

            #region 操作日志

            var moduleDto = module.ToDto();

            OperateRecorder.RecordOperation(oldDTO.Id.ToString(),
                UserSystemMessagesResources.Update_Module,
                moduleDto.GetOperationLog(oldDTO));

            #endregion

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public void Remove(Guid id)
        {
            var module = _Repository.Get(id);

            if (module == null)
            {
                throw new DataNotFoundException(UserSystemMessagesResources.Module_NotExists);
            }

            var moduleDto = module.ToDto();

            _Repository.Remove(module);

            #region 操作日志

            OperateRecorder.RecordOperation(moduleDto.Id.ToString(),
                UserSystemMessagesResources.Remove_Module,
                moduleDto.GetOperationLog());

            #endregion

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public ModuleDTO FindBy(Guid id)
        {
            return _Repository.Get(id).ToDto();
        }

        public IPagedList<ModuleDTO> FindBy(string name, int pageNumber, int pageSize)
        {
            var list = _Repository.FindBy(name, pageNumber, pageSize);

            var result = list.OrderBy(x => x.SortOrder).ToList();

            return new StaticPagedList<ModuleDTO>(
                result.Select(x => x.ToDto()),
                pageNumber,
                pageSize,
                list.TotalItemCount);
        }

        public List<ModuleDTO> ListAll()
        {
            return _Repository.FindAll().OrderBy(x => x.SortOrder).Select(x => x.ToDto()).ToList();
        }
    }
}
