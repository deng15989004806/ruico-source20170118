using System;
using System.Collections.Generic;
using PagedList;
using Ruico.Dto.Common;
using Ruico.Dto.UserSystem;

namespace Ruico.Application.UserSystemModule
{
    public interface IRoleGroupService
    {
        RoleGroupDTO Add(RoleGroupDTO roleGroupDTO);

        void Update(RoleGroupDTO roleGroupDTO);

        void Remove(Guid id);

        List<RoleGroupDTO> FindAll();

        RoleGroupDTO FindBy(Guid id);

        IPagedList<RoleGroupDTO> FindBy(string name, int pageNumber, int pageSize);

        List<IdNameDTO> GetUsersIdName(Guid groupId);

        void UpdateGroupUsers(Guid id, List<Guid> users);
    }
}
