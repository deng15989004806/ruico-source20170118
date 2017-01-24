using System;
using System.Linq;
using Ruico.Domain.UserSystemModule.Entities;
using Ruico.Domain.UserSystemModule.Repositories;
using Ruico.Infrastructure.UnitOfWork;

namespace Ruico.Repository.UserSystemModule.Repositories
{
    public class PermissionRepository : EfRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public bool Exists(Permission item)
        {
            IQueryable<Permission> entities = Table;
            entities = entities.Where(x => x.Menu.Id == item.Menu.Id && x.Name == item.Name);
            if(item.Id != Guid.Empty)
            {
                entities = entities.Where(x => x.Id != item.Id);
            }
            return entities.Any();
        }
    }
}
