using System;
using System.Linq;
using EntityFramework.Extensions;
using PagedList;
using Ruico.Domain.HrModule.Entities;
using Ruico.Domain.HrModule.Repositories;
using Ruico.Infrastructure.UnitOfWork;
using Ruico.Infrastructure.Utility.Helper;

namespace Ruico.Repository.HrModule.Repositories
{
    public class DepartmentRepository : EfRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public IPagedList<Department> FindBy(int? parentId, string name, int pageNumber, int pageSize)
        {
            IQueryable<Department> entities = Table;

            if (name.NotNullOrBlank())
            {
                entities =
                    entities.Where(x => x.Name.Contains(name));
            }
            if (parentId.HasValue)
            {
                entities =
                    entities.Where(x => x.ParentId == parentId.Value);
            }

            var totalCountQuery = entities.FutureCount();
            var resultQuery = entities
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.Created)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Future();

            var totalCount = totalCountQuery.Value;
            var result = resultQuery.ToList();

            return new StaticPagedList<Department>(
                result,
                pageNumber,
                pageSize,
                totalCount);
        }

        public bool Exists(Department item)
        {
            IQueryable<Department> entities = Table;
            entities = entities.Where(x => x.Name == item.Name && x.ParentId == item.ParentId);
            if(item.Id != Guid.Empty)
            {
                entities = entities.Where(x => x.Id != item.Id);
            }
            return entities.Any();
        }
    }
}
