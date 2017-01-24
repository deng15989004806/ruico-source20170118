using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ruico.Application.Resources.Generated;
using Ruico.Dto.Hr;

namespace Ruico.Application.Extensions
{
    public static class HrOperationLogExtension
    {
        public static string GetOperationLog(this DepartmentDTO entity, DepartmentDTO oldDTO = null)
        {
            var sb = new StringBuilder();

            if (oldDTO == null)
            {
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.Name, entity.Name));
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.SortOrder, entity.SortOrder));
                sb.AppendLine(string.Format("{0}: {1}", HrMessagesResources.DepartmentId, entity.DepartmentId));
                sb.AppendLine(string.Format("{0}: {1}", HrMessagesResources.ParentId, entity.ParentId));
            }
            else
            {
                if (entity.Name != oldDTO.Name)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.Name, oldDTO.Name, entity.Name));
                }
                if (entity.SortOrder != oldDTO.SortOrder)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.SortOrder, oldDTO.SortOrder, entity.SortOrder));
                }
                if (entity.DepartmentId != oldDTO.DepartmentId)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        HrMessagesResources.DepartmentId, oldDTO.DepartmentId, entity.DepartmentId));
                }
                if (entity.ParentId != oldDTO.ParentId)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        HrMessagesResources.ParentId, oldDTO.ParentId, entity.ParentId));
                }
            }

            return sb.ToString().TrimEnd('\r', '\n');
        }

        public static string GetOperationLog(this MemberDTO entity, MemberDTO oldDTO = null)
        {
            var sb = new StringBuilder();

            if (oldDTO == null)
            {
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.Name, entity.Name));
                sb.AppendLine(string.Format("{0}: {1}", "Userid", entity.Userid));
                sb.AppendLine(string.Format("{0}: {1}", "WeixinId", entity.WeixinId));
                sb.AppendLine(string.Format("{0}: {1}", "Position", entity.Position));
                sb.AppendLine(string.Format("{0}: {1}", "Gender", entity.Gender));
                sb.AppendLine(string.Format("{0}: {1}", "Mobile", entity.Mobile));
                sb.AppendLine(string.Format("{0}: {1}", "Email", entity.Email));
                sb.AppendLine(string.Format("{0}: {1}", "Status", entity.Status));
                sb.AppendLine(string.Format("{0}: {1}", "Avatar", entity.Avatar));
                sb.AppendLine(string.Format("{0}: {1}", "Enable", entity.Enable));
            }
            else
            {
                if (entity.Name != oldDTO.Name)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.Name, oldDTO.Name, entity.Name));
                }
                if (entity.Userid != oldDTO.Userid)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "Userid", oldDTO.Userid, entity.Userid));
                }
                if (entity.WeixinId != oldDTO.WeixinId)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "WeixinId", oldDTO.WeixinId, entity.WeixinId));
                }
                if (entity.Position != oldDTO.Position)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "Position", oldDTO.Position, entity.Position));
                }
                if (entity.Gender != oldDTO.Gender)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "Gender", oldDTO.Gender, entity.Gender));
                }
                if (entity.Mobile != oldDTO.Mobile)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "Mobile", oldDTO.Mobile, entity.Mobile));
                }
                if (entity.Email != oldDTO.Email)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "Email", oldDTO.Email, entity.Email));
                }
                if (entity.Status != oldDTO.Status)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "Status", oldDTO.Status, entity.Status));
                }
                if (entity.Avatar != oldDTO.Avatar)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "Avatar", oldDTO.Avatar, entity.Avatar));
                }
                if (entity.Enable != oldDTO.Enable)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        "Enable", oldDTO.Enable, entity.Enable));
                }
            }

            return sb.ToString().TrimEnd('\r', '\n');
        }
    }
}
