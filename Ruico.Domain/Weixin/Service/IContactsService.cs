using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ruico.Domain.Weixin.Model;

namespace Ruico.Domain.Weixin.Service
{
    /// <summary>
    /// 通讯录管理
    /// </summary>
    public interface IContactsService
    {
        List<Department> GetDepartments(string accessToken);

        QyCallResult CreateDepartment(string accessToken, Department department);

        QyCallResult UpdateDepartment(string accessToken, Department department);

        QyCallResult DeleteDepartment(string accessToken, int departmentId);

        List<Member> GetMembers(string accessToken);

        Member GetMemberInfo(string accessToken, string userId);

        QyCallResult CreateMember(string accessToken, Member member);

        QyCallResult UpdateMember(string accessToken, Member member);

        QyCallResult DeleteMember(string accessToken, string userId);

        QyCallResult InviteMember(string accessToken, string userId);
    }
}
