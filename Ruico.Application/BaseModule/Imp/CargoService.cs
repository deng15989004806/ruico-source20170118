using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using Ruico.Application.Exceptions;
using Ruico.Application.Extensions;
using Ruico.Application.Resources.Generated;
using Ruico.Application.SystemModule.Imp;
using Ruico.Domain.BaseModule.Entities;
using Ruico.Domain.BaseModule.Repositories;
using Ruico.Dto.Base;
using Ruico.Dto.Base.Converters;
using Ruico.Infrastructure.Entity;
using Ruico.Infrastructure.Utility.Helper;

namespace Ruico.Application.BaseModule.Imp
{
    public class CargoService : ICargoService
    {
        ICargoRepository _Repository;
        ICategoryRepository _CategoryRepository;

        #region Constructors

        public CargoService(ICargoRepository repository,
            ICategoryRepository categoryRepository)                               
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            _Repository = repository;
            _CategoryRepository = categoryRepository;
        }

        #endregion

        public CargoDTO Add(CargoDTO cargoDTO)
        {
            var cargo = cargoDTO.ToModel();
            cargo.Id = IdentityGenerator.NewSequentialGuid();
            cargo.Created = DateTime.UtcNow;
            if (cargoDTO.Category != null)
            {
                cargo.Category = _CategoryRepository.Get(cargoDTO.Category.Id);
            }
            else
            {
                cargo.Category = null;
            }

            if (cargo.Name.IsNullOrBlank())
            {
                throw new DefinedException(CommonMessageResources.Name_Empty);
            }

            if (_Repository.Exists(cargo))
            {
                throw new DataExistsException(string.Format(BaseMessagesResources.Cargo_Exists_WithValue, cargo.Name));
            }

            var ruleName = CommonMessageResources.SerialNumberRule_Cargo;
            var snRule = SerialNumberRuleQuerier.FindBy(ruleName);
            if (snRule == null)
            {
                throw new DataNotFoundException(string.Format(BaseMessagesResources.SerialNumberRule_NotExists_WithValue, ruleName));
            }
            cargo.Sn = SerialNumberGenerator.GetSerialNumber(snRule.Prefix, snRule.UseDateNumber, snRule.NumberLength);

            _Repository.Add(cargo);

            #region 操作日志

            var cargoDto = cargo.ToDto();

            OperateRecorder.RecordOperation(cargoDto.Sn,
                BaseMessagesResources.Add_Cargo,
                cargoDto.GetOperationLog());

            #endregion

            //commit the unit of work
            _Repository.UnitOfWork.Commit();

            return cargo.ToDto();
        }

        public void Update(CargoDTO cargoDTO)
        {
            //get persisted item
            var cargo = _Repository.Get(cargoDTO.Id);

            if (cargo == null)
            {
                throw new DataNotFoundException(BaseMessagesResources.Cargo_NotExists);
            }

            var oldDTO = cargo.ToDto();

            var current = cargoDTO.ToModel();

            cargo.Name = current.Name;
            cargo.Description = current.Description;
            if (cargoDTO.Category != null)
            {
                cargo.Category = _CategoryRepository.Get(cargoDTO.Category.Id);
            }
            else
            {
                cargo.Category = null;
            }

            if (cargo.Name.IsNullOrBlank())
            {
                throw new DefinedException(CommonMessageResources.Name_Empty);
            }

            if (_Repository.Exists(cargo))
            {
                throw new DataExistsException(string.Format(BaseMessagesResources.Cargo_Exists_WithValue, cargo.Name));
            }

            #region 操作日志

            var cargoDto = cargo.ToDto();

            OperateRecorder.RecordOperation(cargoDto.Sn,
                BaseMessagesResources.Update_Cargo,
                cargoDto.GetOperationLog(oldDTO));

            #endregion

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public void Remove(Guid id)
        {
            var cargo = _Repository.Get(id);

            if (cargo == null)
            {
                throw new DataNotFoundException(BaseMessagesResources.Cargo_NotExists);
            }

            var cargoDto = cargo.ToDto();

            _Repository.Remove(cargo);

            #region 操作日志

            OperateRecorder.RecordOperation(cargoDto.Sn,
                BaseMessagesResources.Remove_Cargo,
                cargoDto.GetOperationLog());

            #endregion

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public CargoDTO FindBy(Guid id)
        {
            var cargo = _Repository.Get(id);

            if (cargo == null)
            {
                throw new DataNotFoundException(BaseMessagesResources.Cargo_NotExists);
            }

            return cargo.ToDto();
        }

        public IPagedList<CargoDTO> FindBy(string name, Guid? categoryId, int pageNumber, int pageSize)
        {
            var list = _Repository.FindBy(name, categoryId, pageNumber, pageSize);

            var result = list.ToList();

            return new StaticPagedList<CargoDTO>(
                result.Select(x => x.ToDto()),
                pageNumber,
                pageSize,
                list.TotalItemCount);
        }

        public IList<CategoryDTO> GetCargoCategories()
        {
            var category = _CategoryRepository.Find(x => x.Depth == 1 && x.Name == CommonMessageResources.Category_Cargo);

            return category == null ? new List<CategoryDTO>() : 
                (category.Children ?? new List<Category>()).Select(x => x.ToDto()).ToList();
        }
    }
}
