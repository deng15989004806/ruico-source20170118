using System;
using System.Linq;
using EntityFramework.Extensions;
using PagedList;
using Ruico.Domain.BaseModule.Entities;
using Ruico.Domain.BaseModule.Repositories;
using Ruico.Infrastructure.UnitOfWork;
using Ruico.Infrastructure.Utility.Helper;

namespace Ruico.Repository.BaseModule.Repositories
{
    public class CategoryRepository : EfRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public IPagedList<Category> FindBy(string name, int depth, Guid? parentId, int pageNumber, int pageSize)
        {
            IQueryable<Category> entities = Table; 
            
            if (name.NotNullOrBlank())
            {
                entities =
                    entities.Where(x => x.Name.Contains(name));
            }
            if (depth > 0)
            {
                entities = entities.Where(x => x.Depth == depth);
            }
            if (parentId.HasValue)
            {
                entities =
                    entities.Where(x => x.Parent != null && x.Parent.Id == parentId);
            }

            var totalCountQuery = entities.FutureCount();
            var resultQuery = entities
                .OrderBy(x => x.Depth)
                .ThenBy(x => x.Sn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Future();

            var totalCount = totalCountQuery.Value;
            var result = resultQuery.ToList();

            return new StaticPagedList<Category>(
                result,
                pageNumber,
                pageSize,
                totalCount);
        }

        public bool Exists(Category item)
        {
            IQueryable<Category> entities = Table;
            var parentId = Guid.Empty;
            if (item.Parent != null)
            {
                parentId = item.Parent.Id;
            }
            entities = entities.Where(x => x.Name == item.Name);
            if (parentId == Guid.Empty)
            {
                entities = entities.Where(x => x.Parent == null);
            }
            else
            {
                entities = entities.Where(x => x.Parent != null && x.Parent.Id == parentId);
            }
            if(item.Id != Guid.Empty)
            {
                entities = entities.Where(x => x.Id != item.Id);
            }
            return entities.Any();
        }
    }
}
