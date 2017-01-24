using System;
using System.Linq;
using EntityFramework.Extensions;
using PagedList;
using Ruico.Domain.WeixinModule.Entities;
using Ruico.Domain.WeixinModule.Repositories;
using Ruico.Infrastructure.UnitOfWork;
using Ruico.Infrastructure.Utility.Helper;

namespace Ruico.Repository.WeixinModule.Repositories
{
    public class AppMenuRepository : EfRepository<AppMenu>, IAppMenuRepository
    {
        public AppMenuRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public IPagedList<AppMenu> FindBy(int? appId, string name, int pageNumber, int pageSize)
        {
            IQueryable<AppMenu> entities = Table;

            if (name.NotNullOrBlank())
            {
                entities =
                    entities.Where(x => x.Name.Contains(name));
            }
            if (appId.HasValue)
            {
                entities =
                    entities.Where(x => x.AppId == appId);
            }

            var totalCountQuery = entities.FutureCount();
            var resultQuery = entities
                .OrderBy(x => x.AppId)
                .ThenBy(x => x.SortOrder)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Future();

            var totalCount = totalCountQuery.Value;
            var result = resultQuery.ToList();

            return new StaticPagedList<AppMenu>(
                result,
                pageNumber,
                pageSize,
                totalCount);
        }

        public bool Exists(AppMenu item)
        {
            var parentId = item.Parent == null ? Guid.Empty : item.Parent.Id;

            IQueryable<AppMenu> entities = Table;
            entities = entities.Where(x => x.Name == item.Name && x.AppId == item.AppId);
            if (item.Parent == null)
            {
                entities = entities.Where(x => x.Parent == null);
            }
            else
            {
                entities = entities.Where(x => x.Parent != null && x.Parent.Id == parentId);
            }
            if (item.Id != Guid.Empty)
            {
                entities = entities.Where(x => x.Id != item.Id);
            }
            return entities.Any();
        }
    }
}
