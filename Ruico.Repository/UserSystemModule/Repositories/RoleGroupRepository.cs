using System;
using System.Linq;
using EntityFramework.Extensions;
using PagedList;
using Ruico.Domain.UserSystemModule.Entities;
using Ruico.Domain.UserSystemModule.Repositories;
using Ruico.Infrastructure.UnitOfWork;
using Ruico.Infrastructure.Utility.Helper;

namespace Ruico.Repository.UserSystemModule.Repositories
{
    public class RoleGroupRepository : EfRepository<RoleGroup>, IRoleGroupRepository
    {
        public RoleGroupRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public IPagedList<RoleGroup> FindBy(string name, int pageNumber, int pageSize)
        {
            IQueryable<RoleGroup> entities = Table; 
            
            if (name.NotNullOrBlank())
            {
                entities =
                    entities.Where(x => x.Name.Contains(name));
            }

            var totalCountQuery = entities.FutureCount();
            var resultQuery = entities
                .OrderBy(x => x.SortOrder)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Future();

            var totalCount = totalCountQuery.Value;
            var result = resultQuery.ToList();

            return new StaticPagedList<RoleGroup>(
                result,
                pageNumber,
                pageSize,
                totalCount);
        }

        public bool Exists(RoleGroup item)
        {
            IQueryable<RoleGroup> entities = Table;
            entities = entities.Where(x => x.Name == item.Name);
            if (item.Id != Guid.Empty)
            {
                entities = entities.Where(x => x.Id != item.Id);
            }
            return entities.Any();
        }
    }
}
