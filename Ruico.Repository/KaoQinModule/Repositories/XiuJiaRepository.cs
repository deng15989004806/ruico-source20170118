using System;
using System.Collections.Generic;
using System.Linq;
using EntityFramework.Extensions;
using PagedList;
using Ruico.Domain.KaoQinModule.Entities;
using Ruico.Domain.KaoQinModule.Repositories;
using Ruico.Infrastructure.UnitOfWork;
using Ruico.Infrastructure.Utility.Extensions;
using Ruico.Infrastructure.Utility.Helper;

namespace Ruico.Repository.KaoQinModule.Repositories
{
    public class XiuJiaRepository : EfRepository<XiuJia>, IXiuJiaRepository
    {
        public XiuJiaRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public IPagedList<XiuJia> FindBy(KaoQinCondition condition, int pageNumber, int pageSize)
        {
            IQueryable<XiuJia> entities = Table;

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

            return new StaticPagedList<XiuJia>(
                result,
                pageNumber,
                pageSize,
                totalCount);
        }

        public bool Exists(XiuJia item)
        {
            IQueryable<XiuJia> entities = Table;
            entities = entities.Where(x => x.Name == item.Name && x.ActionStartTime == item.ActionStartTime);
            if(item.Id != Guid.Empty)
            {
                entities = entities.Where(x => x.Id != item.Id);
            }
            return entities.Any();
        }
    }
}
