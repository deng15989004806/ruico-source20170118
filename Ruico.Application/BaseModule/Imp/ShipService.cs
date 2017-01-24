using System;
using System.Linq;
using PagedList;
using Ruico.Application.Exceptions;
using Ruico.Application.Extensions;
using Ruico.Application.Resources.Generated;
using Ruico.Application.SystemModule.Imp;
using Ruico.Domain.BaseModule.Repositories;
using Ruico.Dto.Base;
using Ruico.Dto.Base.Converters;
using Ruico.Infrastructure.Entity;
using Ruico.Infrastructure.Utility.Helper;

namespace Ruico.Application.BaseModule.Imp
{
    public class ShipService : IShipService
    {
        IShipRepository _Repository;

        #region Constructors

        public ShipService(IShipRepository repository)                               
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            _Repository = repository;
        }

        #endregion

        public ShipDTO Add(ShipDTO shipDTO)
        {
            var ship = shipDTO.ToModel();
            ship.Id = IdentityGenerator.NewSequentialGuid();
            ship.Created = DateTime.UtcNow;

            if (ship.Name.IsNullOrBlank())
            {
                throw new DefinedException(CommonMessageResources.Name_Empty);
            }

            if (_Repository.Exists(ship))
            {
                throw new DataExistsException(string.Format(BaseMessagesResources.Ship_Exists_WithValue, ship.Name));
            }

            var ruleName = CommonMessageResources.SerialNumberRule_Ship;
            var snRule = SerialNumberRuleQuerier.FindBy(ruleName);
            if (snRule == null)
            {
                throw new DataNotFoundException(string.Format(BaseMessagesResources.SerialNumberRule_NotExists_WithValue, ruleName));
            }
            ship.Sn = SerialNumberGenerator.GetSerialNumber(snRule.Prefix, snRule.UseDateNumber, snRule.NumberLength);

            _Repository.Add(ship);

            #region 操作日志

            var shipDto = ship.ToDto();

            OperateRecorder.RecordOperation(shipDto.Sn,
                BaseMessagesResources.Add_Ship,
                shipDto.GetOperationLog());

            #endregion

            //commit the unit of work
            _Repository.UnitOfWork.Commit();

            return ship.ToDto();
        }

        public void Update(ShipDTO shipDTO)
        {
            //get persisted item
            var ship = _Repository.Get(shipDTO.Id);

            if (ship == null)
            {
                throw new DataNotFoundException(BaseMessagesResources.Ship_NotExists);
            }

            var oldDTO = ship.ToDto();

            var current = shipDTO.ToModel();

            ship.Name = current.Name;
            ship.Description = current.Description;

            if (ship.Name.IsNullOrBlank())
            {
                throw new DefinedException(CommonMessageResources.Name_Empty);
            }

            if (_Repository.Exists(ship))
            {
                throw new DataExistsException(string.Format(BaseMessagesResources.Ship_Exists_WithValue, ship.Name));
            }

            #region 操作日志

            var shipDto = ship.ToDto();

            OperateRecorder.RecordOperation(shipDto.Sn,
                BaseMessagesResources.Update_Ship,
                shipDto.GetOperationLog(oldDTO));

            #endregion

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public void Remove(Guid id)
        {
            var ship = _Repository.Get(id);

            if (ship == null)
            {
                throw new DataNotFoundException(BaseMessagesResources.Ship_NotExists);
            }

            var shipDto = ship.ToDto();

            _Repository.Remove(ship);

            #region 操作日志

            OperateRecorder.RecordOperation(shipDto.Sn,
                BaseMessagesResources.Remove_Ship,
                shipDto.GetOperationLog());

            #endregion

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public ShipDTO FindBy(Guid id)
        {
            var ship = _Repository.Get(id);

            if (ship == null)
            {
                throw new DataNotFoundException(BaseMessagesResources.Ship_NotExists);
            }

            return ship.ToDto();
        }

        public IPagedList<ShipDTO> FindBy(string name, int pageNumber, int pageSize)
        {
            var list = _Repository.FindBy(name, pageNumber, pageSize);

            var result = list.ToList();

            return new StaticPagedList<ShipDTO>(
                result.Select(x => x.ToDto()),
                pageNumber,
                pageSize,
                list.TotalItemCount);
        }
    }
}
