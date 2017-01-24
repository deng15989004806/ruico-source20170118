using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ruico.Dto.KaoQin;

namespace Ruico.Application.Extensions
{
    public static class KaoQinOperationLogExtension
    {
        public static string GetStatusText(this KaoQinStatusDTO staus)
        {
            switch (staus)
            {
                case KaoQinStatusDTO.Submited:
                    return "已提交";
                case KaoQinStatusDTO.Canceled:
                    return "已取消";
                case KaoQinStatusDTO.Approved:
                    return "已审核";
                default:
                    return staus.ToString();
            }
        }

        public static string GetOperationLog(this ChuChaiDTO entity, ChuChaiDTO oldDTO = null)
        {
            var sb = new StringBuilder();

            if (oldDTO == null)
            {
                sb.AppendLine(string.Format("{0}: {1}", "员工编号", entity.UserId));
                sb.AppendLine(string.Format("{0}: {1}", "姓名", entity.Name));
                sb.AppendLine(string.Format("{0}: {1}", "部门", entity.Department));
                sb.AppendLine(string.Format("{0}: {1}", "职位", entity.Position));
                sb.AppendLine(string.Format("{0}: {1}", "出差时间", entity.OutTime));
                sb.AppendLine(string.Format("{0}: {1}", "返回时间", entity.InTime));
                sb.AppendLine(string.Format("{0}: {1}", "出差地点", entity.OutPlace));
                sb.AppendLine(string.Format("{0}: {1}", "出差工作事由", entity.OutReason));
                sb.AppendLine(string.Format("{0}: {1}", "状态", entity.Status));
            }
            else
            {
                if (entity.DepartmentOpinion != oldDTO.DepartmentOpinion)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "部门意见", oldDTO.DepartmentOpinion, entity.DepartmentOpinion));
                }
                if (entity.DepartmentOpinionApproverId != oldDTO.DepartmentOpinionApproverId)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "部门意见审批人", oldDTO.DepartmentOpinionApproverId, entity.DepartmentOpinionApproverId));
                }
                if (entity.GeneralManagerOfficeOpinion != oldDTO.GeneralManagerOfficeOpinion)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "总经办意见", oldDTO.GeneralManagerOfficeOpinion, entity.GeneralManagerOfficeOpinion));
                }
                if (entity.GeneralManagerOfficeOpinionApproverId != oldDTO.GeneralManagerOfficeOpinionApproverId)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "总经办意见审批人", oldDTO.GeneralManagerOfficeOpinionApproverId, entity.GeneralManagerOfficeOpinionApproverId));
                }
                if (entity.CompanyLeaderOpinion != oldDTO.CompanyLeaderOpinion)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "公司领导意见", oldDTO.CompanyLeaderOpinion, entity.CompanyLeaderOpinion));
                }
                if (entity.CompanyLeaderOpinionApproverId != oldDTO.CompanyLeaderOpinionApproverId)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "公司领导意见审批人", oldDTO.CompanyLeaderOpinionApproverId, entity.CompanyLeaderOpinionApproverId));
                }
                if (entity.Status != oldDTO.Status)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "状态", oldDTO.Status, entity.Status));
                }
            }

            return sb.ToString().TrimEnd('\r', '\n');
        }

        public static string GetOperationLog(this WaiQinDTO entity, WaiQinDTO oldDTO = null)
        {
            var sb = new StringBuilder();

            if (oldDTO == null)
            {
                sb.AppendLine(string.Format("{0}: {1}", "员工编号", entity.UserId));
                sb.AppendLine(string.Format("{0}: {1}", "姓名", entity.Name));
                sb.AppendLine(string.Format("{0}: {1}", "部门", entity.Department));
                sb.AppendLine(string.Format("{0}: {1}", "职位", entity.Position));
                sb.AppendLine(string.Format("{0}: {1}", "外出时间", entity.OutTime));
                sb.AppendLine(string.Format("{0}: {1}", "返回时间", entity.InTime));
                sb.AppendLine(string.Format("{0}: {1}", "外出地点", entity.OutPlace));
                sb.AppendLine(string.Format("{0}: {1}", "外出工作事由", entity.OutReason));
                sb.AppendLine(string.Format("{0}: {1}", "状态", entity.Status));
            }
            else
            {
                if (entity.DepartmentOpinion != oldDTO.DepartmentOpinion)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "部门意见", oldDTO.DepartmentOpinion, entity.DepartmentOpinion));
                }
                if (entity.DepartmentOpinionApproverId != oldDTO.DepartmentOpinionApproverId)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "部门意见审批人", oldDTO.DepartmentOpinionApproverId, entity.DepartmentOpinionApproverId));
                }
                if (entity.GeneralManagerOfficeOpinion != oldDTO.GeneralManagerOfficeOpinion)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "总经办意见", oldDTO.GeneralManagerOfficeOpinion, entity.GeneralManagerOfficeOpinion));
                }
                if (entity.GeneralManagerOfficeOpinionApproverId != oldDTO.GeneralManagerOfficeOpinionApproverId)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "总经办意见审批人", oldDTO.GeneralManagerOfficeOpinionApproverId, entity.GeneralManagerOfficeOpinionApproverId));
                }
                if (entity.CompanyLeaderOpinion != oldDTO.CompanyLeaderOpinion)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "公司领导意见", oldDTO.CompanyLeaderOpinion, entity.CompanyLeaderOpinion));
                }
                if (entity.CompanyLeaderOpinionApproverId != oldDTO.CompanyLeaderOpinionApproverId)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "公司领导意见审批人", oldDTO.CompanyLeaderOpinionApproverId, entity.CompanyLeaderOpinionApproverId));
                }
                if (entity.Status != oldDTO.Status)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "状态", oldDTO.Status, entity.Status));
                }
            }

            return sb.ToString().TrimEnd('\r', '\n');
        }

        public static string GetOperationLog(this WeiDaKaDTO entity, WeiDaKaDTO oldDTO = null)
        {
            var sb = new StringBuilder();

            if (oldDTO == null)
            {
                sb.AppendLine(string.Format("{0}: {1}", "员工编号", entity.UserId));
                sb.AppendLine(string.Format("{0}: {1}", "姓名", entity.Name));
                sb.AppendLine(string.Format("{0}: {1}", "部门", entity.Department));
                sb.AppendLine(string.Format("{0}: {1}", "职位", entity.Position));
                sb.AppendLine(string.Format("{0}: {1}", "填写时间", entity.InputTime));
                sb.AppendLine(string.Format("{0}: {1}", "未打卡时间", entity.ActionTime));
                sb.AppendLine(string.Format("{0}: {1}", "未打卡类型", entity.Type));
                sb.AppendLine(string.Format("{0}: {1}", "未打卡原因", entity.Reason));
                sb.AppendLine(string.Format("{0}: {1}", "状态", entity.Status));
            }
            else
            {
                if (entity.DepartmentOrCompanyOpinion != oldDTO.DepartmentOrCompanyOpinion)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "部门/公司意见", oldDTO.DepartmentOrCompanyOpinion, entity.DepartmentOrCompanyOpinion));
                }
                if (entity.DepartmentOrCompanyOpinionApproverId != oldDTO.DepartmentOrCompanyOpinionApproverId)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "部门/公司意见审批人", oldDTO.DepartmentOrCompanyOpinionApproverId, entity.DepartmentOrCompanyOpinionApproverId));
                }
                if (entity.Status != oldDTO.Status)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "状态", oldDTO.Status, entity.Status));
                }
            }

            return sb.ToString().TrimEnd('\r', '\n');
        }

        public static string GetOperationLog(this XiuJiaDTO entity, XiuJiaDTO oldDTO = null)
        {
            var sb = new StringBuilder();

            if (oldDTO == null)
            {
                sb.AppendLine(string.Format("{0}: {1}", "员工编号", entity.UserId));
                sb.AppendLine(string.Format("{0}: {1}", "姓名", entity.Name));
                sb.AppendLine(string.Format("{0}: {1}", "部门", entity.Department));
                sb.AppendLine(string.Format("{0}: {1}", "职位", entity.Position));
                sb.AppendLine(string.Format("{0}: {1}", "填写时间", entity.InputTime));
                sb.AppendLine(string.Format("{0}: {1}", "休假起始时间", entity.ActionStartTime));
                sb.AppendLine(string.Format("{0}: {1}", "休假结束时间", entity.ActionEndTime));
                sb.AppendLine(string.Format("{0}: {1}", "休假类型", entity.Type));
                sb.AppendLine(string.Format("{0}: {1}", "请假原因", entity.Reason));
                sb.AppendLine(string.Format("{0}: {1}", "休假天数", entity.ActionDays));
                sb.AppendLine(string.Format("{0}: {1}", "休假小时数", entity.ActionHours));
                sb.AppendLine(string.Format("{0}: {1}", "状态", entity.Status));
            }
            else
            {
                if (entity.DepartmentSupervisorOpinion != oldDTO.DepartmentSupervisorOpinion)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "部门主管意见", oldDTO.DepartmentSupervisorOpinion, entity.DepartmentSupervisorOpinion));
                }
                if (entity.DepartmentSupervisorOpinionApproverId != oldDTO.DepartmentSupervisorOpinionApproverId)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "部门主管意见审批人", oldDTO.DepartmentSupervisorOpinionApproverId, entity.DepartmentSupervisorOpinionApproverId));
                }
                if (entity.DepartmentManagerOpinion != oldDTO.DepartmentManagerOpinion)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "部门经理意见", oldDTO.DepartmentManagerOpinion, entity.DepartmentManagerOpinion));
                }
                if (entity.DepartmentManagerOpinionApproverId != oldDTO.DepartmentManagerOpinionApproverId)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "部门经理意见审批人", oldDTO.DepartmentManagerOpinionApproverId, entity.DepartmentManagerOpinionApproverId));
                }
                if (entity.CompanyLeaderOpinion != oldDTO.CompanyLeaderOpinion)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "公司领导意见", oldDTO.CompanyLeaderOpinion, entity.CompanyLeaderOpinion));
                }
                if (entity.CompanyLeaderOpinionApproverId != oldDTO.CompanyLeaderOpinionApproverId)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "公司领导意见审批人", oldDTO.CompanyLeaderOpinionApproverId, entity.CompanyLeaderOpinionApproverId));
                }
                if (entity.Status != oldDTO.Status)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "状态", oldDTO.Status, entity.Status));
                }
            }

            return sb.ToString().TrimEnd('\r', '\n');
        }
    }
}
