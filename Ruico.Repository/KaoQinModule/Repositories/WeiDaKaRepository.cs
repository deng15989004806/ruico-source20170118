using System;
using System.Collections.Generic;
using System.Linq;
using EntityFramework.Extensions;
using PagedList;
using Ruico.Domain.KaoQinModule.Entities;
using Ruico.Domain.KaoQinModule.Repositories;
using Ruico.Infrastructure.UnitOfWork;
using Ruico.Infrastructure.Utility.Helper;
using Ruico.Infrastructure.Utility.Extensions;

namespace Ruico.Repository.KaoQinModule.Repositories
{
    public class WeiDaKaRepository : EfRepository<WeiDaKa>, IWeiDaKaRepository
    {
        public WeiDaKaRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public IPagedList<WeiDaKa> FindBy(KaoQinCondition condition, int pageNumber, int pageSize)
        {
            IQueryable<WeiDaKa> entities = Table;

            if (condition != null)
            {
                if (condition.UserId.NotNullOrBlank())
                {
                    entities =
                        entities.Where(x => x.UserId == condition.UserId);
                }
                if (condition.DepartmentIds.OpSafe().Count > 0)
                {
                    entities =
                        entities.Where(x => condition.DepartmentIds.Contains(x.DepartmentId));
                }
                if (condition.Statuses.OpSafe().Count > 0)
                {
                    entities =
                        entities.Where(x => condition.Statuses.Contains(x.Status));
                }
                if (condition.CreatedStartTime.HasValue)
                {
                    entities =
                        entities.Where(x => x.Created >= condition.CreatedStartTime.Value);
                }
                if (condition.CreatedEndTime.HasValue)
                {
                    entities =
                        entities.Where(x => x.Created < condition.CreatedEndTime.Value);
                }
                if (condition.ExcludeUserId.NotNullOrBlank())
                {
                    entities =
                        entities.Where(x => x.UserId != condition.ExcludeUserId);
                }
                if (condition.ExcludePositions.OpSafe().Count > 0)
                {
                    entities =
                        entities.Where(x => !condition.ExcludePositions.Contains(x.Position));
                }
            }

            var totalCountQuery = entities.FutureCount();
            var resultQuery = entities
                .OrderByDescending(x => x.Created)
                .ThenByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Future();

            var totalCount = totalCountQuery.Value;
            var result = resultQuery.ToList();

            return new StaticPagedList<WeiDaKa>(
                result,
                pageNumber,
                pageSize,
                totalCount);
        }

        public bool Exists(WeiDaKa item)
        {
            IQueryable<WeiDaKa> entities = Table;
            entities = entities.Where(x => x.Name == item.Name && x.ActionTime == item.ActionTime && x.Type == item.Type);
            if(item.Id != Guid.Empty)
            {
                entities = entities.Where(x => x.Id != item.Id);
            }
            return entities.Any();
        }
    }
}
