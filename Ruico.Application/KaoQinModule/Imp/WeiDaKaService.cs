﻿using System;
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
    public class WeiDaKaService : IWeiDaKaService
    {
        IWeiDaKaRepository _Repository;

        #region Constructors

        public WeiDaKaService(IWeiDaKaRepository repository)                               
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            _Repository = repository;
        }

        #endregion

        private void ValidateModel(WeiDaKa model)
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

            if (model.Type.IsNullOrBlank())
            {
                throw new DefinedException(KaoQinMessagesResources.WeiDaKa_Type_Empty);
            }

            if (model.Reason.IsNullOrBlank())
            {
                throw new DefinedException(KaoQinMessagesResources.WeiDaKa_Reason_Empty);
            }

            if (_Repository.Exists(model))
            {
                throw new DataExistsException(string.Format(KaoQinMessagesResources.WeiDaKa_Exists_WithValue, model.UserId, model.ActionTime.ToLocalTime().ToString("yyyy-MM-dd ddd"), model.Type));
            }
        }

        /***********
        微信用户操作时，不能通过cookies找到登陆用户，因此，在表现层传当前的微信用户进来作为操作者
        *************/
        private void OperationLog(string action, WeiDaKaDTO itemDto, WeiDaKaDTO oldDto = null, UserDTO operatorDTO = null)
        {
            OperateRecorder.RecordOperation(itemDto.Id.ToString(), action,
                itemDto.GetOperationLog(oldDto), operatorDTO);
        }

        public WeiDaKaDTO Add(WeiDaKaDTO itemDto, UserDTO operatorDTO = null)
        {
            var model = itemDto.ToModel();
            model.Id = IdentityGenerator.NewSequentialGuid();
            model.Created = DateTime.UtcNow;
            model.Canceled = SqlDateTime.MinValue.Value;
            model.Approved = SqlDateTime.MinValue.Value;
            model.InputTime = DateTime.Today;

            model.ActionTime = model.ActionTime.ToUniversalTime();

            // 数据验证
            this.ValidateModel(model);

            model.Status = KaoQinStatusDTO.Submited.ToString(); ;

            _Repository.Add(model);

            this.OperationLog(KaoQinMessagesResources.Add_WeiDaKa, model.ToDto(),  null, operatorDTO);

            //commit the unit of work
            _Repository.UnitOfWork.Commit();

            return model.ToDto();
        }

        public void Approve(WeiDaKaDTO itemDto, UserDTO operatorDTO = null)
        {
            //get persisted item
            var persistedModel = _Repository.Get(itemDto.Id);

            if (persistedModel == null)
            {
                throw new DataNotFoundException(KaoQinMessagesResources.WaiQin_NotExists);
            }

            if (persistedModel.Status == KaoQinStatusDTO.Canceled.ToString())
            {
                throw new DataNotFoundException(KaoQinMessagesResources.Canceled_CanNot_Approved);
            }

            if (itemDto.DepartmentOrCompanyOpinion.IsNullOrBlank())
            {
                throw new DataNotFoundException(KaoQinMessagesResources.Approve_Opinion_Empty);
            }

            if (persistedModel.DepartmentOrCompanyOpinion != itemDto.DepartmentOrCompanyOpinion)
            {
                UpdateDepartmentOrCompanyOpinion(persistedModel, itemDto, operatorDTO);
            }
        }

        private void UpdateDepartmentOrCompanyOpinion(WeiDaKa persistedModel, WeiDaKaDTO itemDto, UserDTO operatorDTO = null)
        {
            var oldDTO = persistedModel.ToDto();

            // 可以修改的字段
            var current = oldDTO.ToModel();
            current.DepartmentOrCompanyOpinion = itemDto.DepartmentOrCompanyOpinion;
            current.DepartmentOrCompanyOpinionApproverId = itemDto.DepartmentOrCompanyOpinionApproverId;

            if (persistedModel.Status == KaoQinStatusDTO.Submited.ToString())
            {
                current.Status = KaoQinStatusDTO.Approved.ToString();
                current.Approved = DateTime.UtcNow;
            }

            this.OperationLog(KaoQinMessagesResources.Update_WeiDaKa, current.ToDto(), oldDTO, operatorDTO);

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
                throw new DataNotFoundException(KaoQinMessagesResources.WeiDaKa_NotExists);
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

                this.OperationLog(KaoQinMessagesResources.Update_WeiDaKa, current.ToDto(), oldDTO, operatorDTO);

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
                throw new DataNotFoundException(KaoQinMessagesResources.WeiDaKa_NotExists);
            }

            var memberDto = persistedModel.ToDto();

            _Repository.Remove(persistedModel);

            this.OperationLog(KaoQinMessagesResources.Remove_WeiDaKa, memberDto, null);

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public WeiDaKaDTO FindBy(Guid id)
        {
            var persistedModel = _Repository.Get(id);

            if (persistedModel == null)
            {
                throw new DataNotFoundException(KaoQinMessagesResources.WeiDaKa_NotExists);
            }

            return persistedModel.ToDto();
        }

        public IPagedList<WeiDaKaDTO> FindBy(KaoQinConditionDTO condition, int pageNumber, int pageSize)
        {
            var list = _Repository.FindBy(condition.ToModel(), pageNumber, pageSize);

            var result = list.ToList();

            return new StaticPagedList<WeiDaKaDTO>(
                result.Select(x => x.ToDto()),
                pageNumber,
                pageSize,
                list.TotalItemCount);
        }

        public DataTable GetExportTable(List<WeiDaKaDTO> items)
        {
            var result = new DataTable("未打卡申请记录");
            result.Columns.Add("员工编号");
            result.Columns.Add("姓名");
            result.Columns.Add("部门");
            result.Columns.Add("职位");
            result.Columns.Add("未打卡时间");
            result.Columns.Add("未打卡类型");
            result.Columns.Add("未打卡原因");
            result.Columns.Add("提交时间");
            result.Columns.Add("状态");
            result.Columns.Add("部门/公司意见");

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
                    dto.ActionTime.ToString("yyyy-MM-dd HH:mm"),
                    dto.Type,
                    dto.Reason,
                    dto.Created.ToString("yyyy-MM-dd HH:mm"),
                    status.GetStatusText(),
                    dto.DepartmentOrCompanyOpinion
                });
            }

            return result;
        }
    }
}
