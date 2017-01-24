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
    public class MenuRepository : EfRepository<Menu>, IMenuRepository
    {
        public MenuRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public IPagedList<Menu> FindBy(Guid moduleId, string name, int pageNumber, int pageSize)
        {
            IQueryable<Menu> entities = Table; 
            
            if (name.NotNullOrBlank())
            {
                entities =
                    entities.Where(x => x.Name.Contains(name));
            }

            if (moduleId != Guid.Empty)
            {
                entities =
                        entities.Where(x => x.Module.Id == moduleId);
            }

            var totalCountQuery = entities.FutureCount();
            var resultQuery = entities
                .OrderBy(x => x.Module.SortOrder)
                .ThenBy(x => x.SortOrder)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Future();

            var totalCount = totalCountQuery.Value;
            var result = resultQuery.ToList();

            return new StaticPagedList<Menu>(
                result,
                pageNumber,
                pageSize,
                totalCount);
        }

        public bool Exists(Menu item)
        {
            var moduleId = item.Module == null ? Guid.Empty : item.Module.Id;
            var parentId = item.Parent == null ? Guid.Empty : item.Parent.Id;

            IQueryable<Menu> entities = Table;
            entities = entities.Where(x => x.Module.Id == moduleId && x.Name == item.Name);
            if (item.Parent == null)
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
