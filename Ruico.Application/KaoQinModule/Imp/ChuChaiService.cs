using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using PagedList;
using Ruico.Application.Exceptions;
using Ruico.Application.Extensions;
using Ruico.Application.Resources.Generated;
using Ruico.Application.SystemModule.Imp;
using Ruico.Domain.KaoQinModule.Entities;
using Ruico.Infrastructure.Entity;
using Ruico.Infrastructure.Utility.Helper;
using Ruico.Domain.KaoQinModule.Repositories;
using Ruico.Dto.KaoQin.Converters;
using Ruico.Dto.KaoQin;
using Ruico.Dto.UserSystem;

namespace Ruico.Application.KaoQinModule.Imp
{
    public class ChuChaiService : IChuChaiService
    {
        IChuChaiRepository _Repository;

        #region Constructors

        public ChuChaiService(IChuChaiRepository repository)                               
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            _Repository = repository;
        }

        #endregion

        private void ValidateModel(ChuChai model)
        {
            if (model.Name.IsNullOrBlank())
            {
                throw new DefinedException(KaoQinMessagesResources.UserName_Empty);
            }

            if (model.UserId.IsNullOrBlank())
            {
                throw new DefinedException(KaoQinMessagesResources.UserId_Empty);
            }

            if (model.DepartmentId == 0)
            {
                throw new DefinedException(KaoQinMessagesResources.DepartmentId_Empty);
            }

            if (model.Position.IsNullOrBlank())
            {
                throw new DefinedException(KaoQinMessagesResources.Position_Empty);
            }

            if (model.OutPlace.IsNullOrBlank())
            {
                throw new DefinedException(KaoQinMessagesResources.ChuChai_OutPlace_Empty);
            }

            if (model.OutReason.IsNullOrBlank())
            {
                throw new DefinedException(KaoQinMessagesResources.ChuChai_OutReason_Empty);
            }

            if (model.OutTime > model.InTime)
            {
                throw new DefinedException(KaoQinMessagesResources.ChuChai_OutTime_Greater_Than_InTime);
            }

            if (_Repository.Exists(model))
            {
                throw new DataExistsException(string.Format(KaoQinMessagesResources.ChuChai_Exists_WithValue, model.UserId, model.OutTime.ToLocalTime()));
            }
        }

        /***********
        微信用户操作时，不能通过cookies找到登陆用户，因此，在表现层传当前的微信用户进来作为操作者
        *************/
        private void OperationLog(string action, ChuChaiDTO itemDto, ChuChaiDTO oldDto = null, UserDTO operatorDTO = null)
        {
            OperateRecorder.RecordOperation(itemDto.Id.ToString(), action,
                itemDto.GetOperationLog(oldDto), operatorDTO);
        }

        public ChuChaiDTO Add(ChuChaiDTO itemDto, UserDTO operatorDTO = null)
        {
            var model = itemDto.ToModel();
            model.Id = IdentityGenerator.NewSequentialGuid();
            model.Created = DateTime.UtcNow;
            model.Canceled = SqlDateTime.MinValue.Value;
            model.Approved = SqlDateTime.MinValue.Value;

            model.OutTime = model.OutTime.ToUniversalTime();
            model.InTime = model.InTime.ToUniversalTime();

            // 数据验证
            this.ValidateModel(model);

            model.Status = KaoQinStatusDTO.Submited.ToString();

            _Repository.Add(model);
            
            this.OperationLog(KaoQinMessagesResources.Add_ChuChai, model.ToDto(),  null, operatorDTO);

            //commit the unit of work
            _Repository.UnitOfWork.Commit();

            return model.ToDto();
        }

        public void Approve(ChuChaiDTO itemDto, UserDTO operatorDTO = null)
        {
            //get persisted item
            var persistedModel = _Repository.Get(itemDto.Id);

            if (persistedModel == null)
            {
                throw new DataNotFoundException(KaoQinMessagesResources.ChuChai_NotExists);
            }

            if (persistedModel.Status == KaoQinStatusDTO.Canceled.ToString())
            {
                throw new DataNotFoundException(KaoQinMessagesResources.Canceled_CanNot_Approved);
            }

            if(itemDto.DepartmentOpinion.IsNullOrBlank()
                && itemDto.GeneralManagerOfficeOpinion.IsNullOrBlank()
                && itemDto.CompanyLeaderOpinion.IsNullOrBlank())
            {
                throw new DataNotFoundException(KaoQinMessagesResources.Approve_Opinion_Empty);
            }

            if(persistedModel.DepartmentOpinion != itemDto.DepartmentOpinion)
            {
                UpdateDepartmentOpinion(persistedModel,itemDto, operatorDTO);
            }
            if (persistedModel.GeneralManagerOfficeOpinion != itemDto.GeneralManagerOfficeOpinion)
            {
                UpdateGeneralManagerOfficeOpinion(persistedModel, itemDto, operatorDTO);
            }
            if (persistedModel.CompanyLeaderOpinion != itemDto.CompanyLeaderOpinion)
            {
                UpdateCompanyLeaderOpinion(persistedModel, itemDto, operatorDTO);
            }

        }

        private void UpdateDepartmentOpinion(ChuChai persistedModel, ChuChaiDTO itemDto, UserDTO operatorDTO = null)
        {
            var oldDTO = persistedModel.ToDto();

            // 可以修改的字段
            var current = oldDTO.ToModel();
            current.DepartmentOpinion = itemDto.DepartmentOpinion;
            current.DepartmentOpinionApproverId = itemDto.DepartmentOpinionApproverId;

            if (persistedModel.Status == KaoQinStatusDTO.Submited.ToString())
            {
                current.Status = KaoQinStatusDTO.Approved.ToString();
                current.Approved = DateTime.UtcNow;
            }
            
            this.OperationLog(KaoQinMessagesResources.Update_ChuChai, current.ToDto(), oldDTO, operatorDTO);

            //Merge changes
            _Repository.Merge(persistedModel, current);
            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        private void UpdateGeneralManagerOfficeOpinion(ChuChai persistedModel, ChuChaiDTO itemDto, UserDTO operatorDTO = null)
        {
            var oldDTO = persistedModel.ToDto();

            // 可以修改的字段
            var current = oldDTO.ToModel();
            current.GeneralManagerOfficeOpinion = itemDto.GeneralManagerOfficeOpinion;
            current.GeneralManagerOfficeOpinionApproverId = itemDto.GeneralManagerOfficeOpinionApproverId;

            if (persistedModel.Status == KaoQinStatusDTO.Submited.ToString())
            {
                current.Status = KaoQinStatusDTO.Approved.ToString();
                current.Approved = DateTime.UtcNow;
            }

            this.OperationLog(KaoQinMessagesResources.Update_ChuChai, current.ToDto(), oldDTO, operatorDTO);

            //Merge changes
            _Repository.Merge(persistedModel, current);
            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        private void UpdateCompanyLeaderOpinion(ChuChai persistedModel, ChuChaiDTO itemDto, UserDTO operatorDTO = null)
        {
            var oldDTO = persistedModel.ToDto();

            // 可以修改的字段
            var current = oldDTO.ToModel();
            current.CompanyLeaderOpinion = itemDto.CompanyLeaderOpinion;
            current.CompanyLeaderOpinionApproverId = itemDto.CompanyLeaderOpinionApproverId;

            if (persistedModel.Status == KaoQinStatusDTO.Submited.ToString())
            {
                current.Status = KaoQinStatusDTO.Approved.ToString();
                current.Approved = DateTime.UtcNow;
            }

            this.OperationLog(KaoQinMessagesResources.Update_ChuChai, current.ToDto(), oldDTO, operatorDTO);

            //Merge changes
            _Repository.Merge(persistedModel, current);
            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public void Cancel(Guid id, UserDTO operatorDTO = null)
        {
            var persistedModel = _Repository.Get(id);

            if (persistedModel == null)
            {
                throw new DataNotFoundException(KaoQinMessagesResources.ChuChai_NotExists);
            }
            
            if (persistedModel.Status == KaoQinStatusDTO.Approved.ToString())
            {
                throw new DataNotFoundException(KaoQinMessagesResources.Approved_CanNot_Canceled);
            }

            if (persistedModel.Status == KaoQinStatusDTO.Submited.ToString())
            {
                var oldDTO = persistedModel.ToDto();

                // 可以修改的字段
                var current = oldDTO.ToModel();
                current.Status = KaoQinStatusDTO.Canceled.ToString();
                current.Canceled = DateTime.UtcNow;

                this.OperationLog(KaoQinMessagesResources.Update_ChuChai, current.ToDto(), oldDTO, operatorDTO);

                //Merge changes
                _Repository.Merge(persistedModel, current);
                //commit unit of work
                _Repository.UnitOfWork.Commit();
            }
        }

        public void Remove(Guid id)
        {
            var persistedModel = _Repository.Get(id);

            if (persistedModel == null)
            {
                throw new DataNotFoundException(KaoQinMessagesResources.ChuChai_NotExists);
            }

            var memberDto = persistedModel.ToDto();

            _Repository.Remove(persistedModel);

            this.OperationLog(KaoQinMessagesResources.Remove_ChuChai, memberDto, null);

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public ChuChaiDTO FindBy(Guid id)
        {
            var persistedModel = _Repository.Get(id);

            if (persistedModel == null)
            {
                throw new DataNotFoundException(KaoQinMessagesResources.ChuChai_NotExists);
            }

            return persistedModel.ToDto();
        }

        public IPagedList<ChuChaiDTO> FindBy(KaoQinConditionDTO condition, int pageNumber, int pageSize)
        {
            var list = _Repository.FindBy(condition.ToModel(), pageNumber, pageSize);

            var result = list.ToList();

            return new StaticPagedList<ChuChaiDTO>(
                result.Select(x => x.ToDto()),
                pageNumber,
                pageSize,
                list.TotalItemCount);
        }

        public DataTable GetExportTable(List<ChuChaiDTO> items)
        {
            var result = new DataTable("出差申请记录");
            result.Columns.Add("员工编号");
            result.Columns.Add("姓名");
            result.Columns.Add("部门");
            result.Columns.Add("职位");
            result.Columns.Add("出差时间");
            result.Columns.Add("返回时间");
            result.Columns.Add("出差地点");
            result.Columns.Add("出差事由");
            result.Columns.Add("指派人");
            result.Columns.Add("提交时间");
            result.Columns.Add("状态");
            result.Columns.Add("部门意见");
            result.Columns.Add("总经办意见");
            result.Columns.Add("公司领导意见");

            foreach (var dto in items)
            {
                KaoQinStatusDTO status;
                Enum.TryParse(dto.Status, true, out status);

                result.Rows.Add(new object[]
                {
                    dto.UserId,
                    dto.Name,
                    dto.Department,
                    dto.Position,
                    dto.OutTime.ToString("yyyy-MM-dd HH:mm"),
                    dto.InTime.ToString("yyyy-MM-dd HH:mm"),
                    dto.OutPlace,
                    dto.OutReason,
                    dto.AppointPerson,
                    dto.Created.ToString("yyyy-MM-dd HH:mm"),
                    status.GetStatusText(),
                    dto.DepartmentOpinion,
                    dto.GeneralManagerOfficeOpinion,
                    dto.CompanyLeaderOpinion
                });
            }

            return result;
        }
    }
}
