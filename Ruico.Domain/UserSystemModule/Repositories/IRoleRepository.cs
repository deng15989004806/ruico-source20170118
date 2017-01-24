using System;
using PagedList;
using Ruico.Domain.UserSystemModule.Entities;
using Ruico.Infrastructure.Repository;

namespace Ruico.Domain.UserSystemModule.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        IPagedList<Role> FindBy(Guid roleGroupId, string name, int pageNumber, int pageSize);

        bool Exists(Role item);
    }
}
