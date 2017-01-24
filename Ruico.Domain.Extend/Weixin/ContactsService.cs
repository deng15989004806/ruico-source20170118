using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ruico.Domain.Weixin.Model;
using Ruico.Domain.Weixin.Service;
using Senparc.Weixin;
using Senparc.Weixin.QY.AdvancedAPIs;

namespace Ruico.Domain.Extend.Weixin
{
    public class ContactsService : IContactsService
    {
        public List<Department> GetDepartments(string accessToken)
        {
            var result = MailListApi.GetDepartmentList(accessToken);
            if (result.errcode != ReturnCode_QY.请求成功)
            {
                throw new Exception(result.errcode.ToString());
            }

            return result.department.Select(x =>
                new Department(x.id, x.name, x.parentid, x.order)).ToList();
        }

        public QyCallResult CreateDepartment(string accessToken, Department department)
        {
            return MailListApi.CreateDepartment(accessToken, department.Name, department.ParentId, department.Order, department.Id).ToQyCallResult();
        }

        public QyCallResult UpdateDepartment(string accessToken, Department department)
        {
            return MailListApi.UpdateDepartment(accessToken, department.Id.ToString(), department.Name, department.ParentId, department.Order).ToQyCallResult();
        }

        public QyCallResult DeleteDepartment(string accessToken, int departmentId)
        {
            return MailListApi.DeleteDepartment(accessToken, departmentId.ToString()).ToQyCallResult();
        }

        public List<Member> GetMembers(string accessToken)
        {
            var result = MailListApi.GetDepartmentMemberInfo(accessToken, 1, 1, 0);
            if (result.errcode != ReturnCode_QY.请求成功)
            {
                throw new Exception(result.errcode.ToString());
            }

            return result.userlist.Select(x =>
                new Member(x.userid, x.name, x.weixinid)
                {
                    Department = new List<int>(x.department),
                    Position = x.position,
                    Gender = x.gender,
                    Mobile = x.mobile,
                    Email = x.email,
                    Status = x.status,
                    Avatar = x.avatar,
                    Enable = x.status == 2 ? 0 : 1
                }).ToList();
        }

        public Member GetMemberInfo(string accessToken, string userId)
        {
            var result = MailListApi.GetMember(accessToken, userId);
            if (result.errcode != ReturnCode_QY.请求成功)
            {
                throw new Exception(result.errcode.ToString());
            }

            var x = result;
            return  new Member(x.userid, x.name, x.weixinid)
                {
                    Department = new List<int>(x.department),
                    Position = x.position,
                    Gender = x.gender,
                    Mobile = x.mobile,
                    Email = x.email,
                    Status = x.status,
                    Avatar = x.avatar,
                    Enable = x.status == 2 ? 0 : 1
                };
        }

        public QyCallResult CreateMember(string accessToken, Member member)
        {
            return
                MailListApi.CreateMember(accessToken, member.Userid, member.Name,
                    member.Department.ToArray(), member.Position, member.Mobile, 
                    member.Email, member.WeixinId).ToQyCallResult();
        }

        public QyCallResult UpdateMember(string accessToken, Member member)
        {
            return
                MailListApi.UpdateMember(accessToken, member.Userid, member.Name,
                    member.Department.ToArray(), member.Position, member.Mobile,
                    member.Email, member.WeixinId, member.Enable).ToQyCallResult();
        }

        public QyCallResult DeleteMember(string accessToken, string userId)
        {
            return MailListApi.DeleteMember(accessToken, userId).ToQyCallResult();
        }

        public QyCallResult InviteMember(string accessToken, string userId)
        {
            return MailListApi.InviteMember(accessToken, userId).ToQyCallResult();
        }
    }
}
