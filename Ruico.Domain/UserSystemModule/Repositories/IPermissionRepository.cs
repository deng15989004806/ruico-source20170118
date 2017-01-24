using Ruico.Domain.UserSystemModule.Entities;
using Ruico.Infrastructure.Repository;

namespace Ruico.Domain.UserSystemModule.Repositories
{
    public interface IPermissionRepository : IRepository<Permission>
    {
        bool Exists(Permission item);
    }
}
