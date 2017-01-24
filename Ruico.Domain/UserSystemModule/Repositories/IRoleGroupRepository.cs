using PagedList;
using Ruico.Domain.UserSystemModule.Entities;
using Ruico.Infrastructure.Repository;

namespace Ruico.Domain.UserSystemModule.Repositories
{
    public interface IRoleGroupRepository : IRepository<RoleGroup>
    {
        IPagedList<RoleGroup> FindBy(string name, int pageNumber, int pageSize);

        bool Exists(RoleGroup item);
    }
}
