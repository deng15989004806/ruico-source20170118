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
    public class XiuJiaService : IXiuJiaService
    {
        IXiuJiaRepository _Repository;

        #region Constructors

        public XiuJiaService(IXiuJiaRepository repository)                               
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            _Repository = repository;
        }

        #endregion

        private void ValidateModel(XiuJia model)
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

            if (model.ActionStartTime > model.ActionEndTime)
            {
                throw new DefinedException(KaoQinMessagesResources.XiuJia_StartTime_Greater_Than_EndTime);
            }

            if (model.Type.IsNullOrBlank())
            {
                throw new DefinedException(KaoQinMessagesResources.XiuJia_Type_Empty);
            }

            if (model.Reason.IsNullOrBlank())
            {
                throw new DefinedException(KaoQinMessagesResources.XiuJia_Reason_Empty);
            }

            if (model.ActionDays <= 0 && model.ActionHours <= 0)
            {
                throw new DefinedException(KaoQinMessagesResources.XiuJia_Days_Hours_Empty);
            }

            if (_Repository.Exists(model))
            {
                throw new DataExistsException(string.Format(KaoQinMessagesResources.XiuJia_Exists_WithValue, model.UserId, model.ActionStartTime.ToLocalTime()));
            }
        }

        /***********
        微信用户操作时，不能通过cookies找到登陆用户，因此，在表现层传当前的微信用户进来作为操作者
        *************/
        private void OperationLog(string action, XiuJiaDTO itemDto, XiuJiaDTO oldDto = null, UserDTO operatorDTO = null)
        {
            OperateRecorder.RecordOperation(itemDto.Id.ToString(), action,
                itemDto.GetOperationLog(oldDto), operatorDTO);
        }

        public XiuJiaDTO Add(XiuJiaDTO itemDto, UserDTO operatorDTO = null)
        {
            var model = itemDto.ToModel();
            model.Id = IdentityGenerator.NewSequentialGuid();
            model.Created = DateTime.UtcNow;
            model.Canceled = SqlDateTime.MinValue.Value;
            model.Approved = SqlDateTime.MinValue.Value;
            model.InputTime = DateTime.Today;

            model.ActionStartTime = model.ActionStartTime.ToUniversalTime();
            model.ActionEndTime = model.ActionEndTime.ToUniversalTime();

            // 数据验证
            this.ValidateModel(model);

            model.Status = KaoQinStatusDTO.Submited.ToString(); ;

            _Repository.Add(model);

            this.OperationLog(KaoQinMessagesResources.Add_XiuJia, model.ToDto(),  null, operatorDTO);

            //commit the unit of work
            _Repository.UnitOfWork.Commit();

            return model.ToDto();
        }

        public void Approve(XiuJiaDTO itemDto, UserDTO operatorDTO = null)
        {
            //get persisted item
            var persistedModel = _Repository.Get(itemDto.Id);

            if (persistedModel == null)
            {
                throw new DataNotFoundException(KaoQinMessagesResources.XiuJia_NotExists);
            }

            if (persistedModel.Status == KaoQinStatusDTO.Canceled.ToString())
            {
                throw new DataNotFoundException(KaoQinMessagesResources.Canceled_CanNot_Approved);
            }

            if (itemDto.DepartmentSupervisorOpinion.IsNullOrBlank()
                && itemDto.DepartmentManagerOpinion.IsNullOrBlank()
                && itemDto.CompanyLeaderOpinion.IsNullOrBlank())
            {
                throw new DataNotFoundException(KaoQinMessagesResources.Approve_Opinion_Empty);
            }

            if (persistedModel.DepartmentSupervisorOpinion != itemDto.DepartmentSupervisorOpinion)
            {
                UpdateDepartmentOpinion(persistedModel, itemDto, operatorDTO);
            }
            if (persistedModel.DepartmentManagerOpinion != itemDto.DepartmentManagerOpinion)
            {
                UpdateGeneralManagerOfficeOpinion(persistedModel, itemDto, operatorDTO);
            }
            if (persistedModel.CompanyLeaderOpinion != itemDto.CompanyLeaderOpinion)
            {
                UpdateCompanyLeaderOpinion(persistedModel, itemDto, operatorDTO);
            }

        }

        private void UpdateDepartmentOpinion(XiuJia persistedModel, XiuJiaDTO itemDto, UserDTO operatorDTO = null)
        {
            var oldDTO = persistedModel.ToDto();

            // 可以修改的字段
            var current = oldDTO.ToModel();
            current.DepartmentSupervisorOpinion = itemDto.DepartmentSupervisorOpinion;
            current.DepartmentSupervisorOpinionApproverId = itemDto.DepartmentSupervisorOpinionApproverId;

            if (persistedModel.Status == KaoQinStatusDTO.Submited.ToString())
            {
                current.Status = KaoQinStatusDTO.Approved.ToString();
                current.Approved = DateTime.UtcNow;
            }

            this.OperationLog(KaoQinMessagesResources.Update_XiuJia, current.ToDto(), oldDTO, operatorDTO);

            //Merge changes
            _Repository.Merge(persistedModel, current);
            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        private void UpdateGeneralManagerOfficeOpinion(XiuJia persistedModel, XiuJiaDTO itemDto, UserDTO operatorDTO = null)
        {
            var oldDTO = persistedModel.ToDto();

            // 可以修改的字段
            var current = oldDTO.ToModel();
            current.DepartmentManagerOpinion = itemDto.DepartmentManagerOpinion;
            current.DepartmentManagerOpinionApproverId = itemDto.DepartmentManagerOpinionApproverId;

            if (persistedModel.Status == KaoQinStatusDTO.Submited.ToString())
            {
                current.Status = KaoQinStatusDTO.Approved.ToString();
                current.Approved = DateTime.UtcNow;
            }

            this.OperationLog(KaoQinMessagesResources.Update_XiuJia, current.ToDto(), oldDTO, operatorDTO);

            //Merge changes
            _Repository.Merge(persistedModel, current);
            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        private void UpdateCompanyLeaderOpinion(XiuJia persistedModel, XiuJiaDTO itemDto, UserDTO operatorDTO = null)
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

            this.OperationLog(KaoQinMessagesResources.Update_XiuJia, current.ToDto(), oldDTO, operatorDTO);

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
                throw new DataNotFoundException(KaoQinMessagesResources.XiuJia_NotExists);
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

                this.OperationLog(KaoQinMessagesResources.Update_XiuJia, current.ToDto(), oldDTO, operatorDTO);

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
                throw new DataNotFoundException(KaoQinMessagesResources.XiuJia_NotExists);
            }

            var memberDto = persistedModel.ToDto();

            _Repository.Remove(persistedModel);

            this.OperationLog(KaoQinMessagesResources.Remove_XiuJia, memberDto, null);

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public XiuJiaDTO FindBy(Guid id)
        {
            var persistedModel = _Repository.Get(id);

            if (persistedModel == null)
            {
                throw new DataNotFoundException(KaoQinMessagesResources.XiuJia_NotExists);
            }

            return persistedModel.ToDto();
        }

        public IPagedList<XiuJiaDTO> FindBy(KaoQinConditionDTO condition, int pageNumber, int pageSize)
        {
            var list = _Repository.FindBy(condition.ToModel(), pageNumber, pageSize);

            var result = list.ToList();

            return new StaticPagedList<XiuJiaDTO>(
                result.Select(x => x.ToDto()),
                pageNumber,
                pageSize,
                list.TotalItemCount);
        }

        public DataTable GetExportTable(List<XiuJiaDTO> items)
        {
            var result = new DataTable("休假申请记录");
            result.Columns.Add("员工编号");
            result.Columns.Add("姓名");
            result.Columns.Add("部门");
            result.Columns.Add("职位");
            result.Columns.Add("请假类别");
            result.Columns.Add("请假事由");
            result.Columns.Add("开始时间");
            result.Columns.Add("结束时间");
            result.Columns.Add("请假天数");
            result.Columns.Add("请假小时数");
            result.Columns.Add("提交时间");
            result.Columns.Add("状态");
            result.Columns.Add("部门主管意见");
            result.Columns.Add("部门经理意见");
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
                    dto.Type,
                    dto.Reason,
                    dto.ActionStartTime.ToString("yyyy-MM-dd HH:mm"),
                    dto.ActionEndTime.ToString("yyyy-MM-dd HH:mm"),
                    dto.ActionDays,
                    dto.ActionHours,
                    dto.Created.ToString("yyyy-MM-dd HH:mm"),
                    status.GetStatusText(),
                    dto.DepartmentSupervisorOpinion,
                    dto.DepartmentManagerOpinion,
                    dto.CompanyLeaderOpinion
                });
            }

            return result;
        }
    }
}
