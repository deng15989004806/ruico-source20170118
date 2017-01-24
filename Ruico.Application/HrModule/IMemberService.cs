using System;
using System.Collections.Generic;
using PagedList;
using Ruico.Dto.Hr;

namespace Ruico.Application.HrModule
{
    public interface IMemberService
    {
        MemberDTO Add(MemberDTO item);

        void Update(MemberDTO item);

        void Remove(Guid id);

        MemberDTO FindBy(Guid id);

        IPagedList<MemberDTO> FindBy(string name, int pageNumber, int pageSize);

        MemberDTO FindByUserId(string userId);

        void UpdateMemberDepartments(Guid memberId, List<Guid> departmentIds);

        void DownloadMembers();

        void UploadMembers();

        void RemoveNotExistMemberInWeixin();

        void InviteMember(List<string> userIds);
    }
}
