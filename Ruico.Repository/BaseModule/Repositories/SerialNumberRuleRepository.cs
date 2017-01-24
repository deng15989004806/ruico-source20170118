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
    public class SerialNumberRuleRepository : EfRepository<SerialNumberRule>, ISerialNumberRuleRepository
    {
        public SerialNumberRuleRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public IPagedList<SerialNumberRule> FindBy(string name, int pageNumber, int pageSize)
        {
            IQueryable<SerialNumberRule> entities = Table; 
            
            if (name.NotNullOrBlank())
            {
                entities =
                    entities.Where(x => x.RuleName.Contains(name));
            }

            var totalCountQuery = entities.FutureCount();
            var resultQuery = entities
                .OrderBy(x => x.RuleName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Future();

            var totalCount = totalCountQuery.Value;
            var result = resultQuery.ToList();

            return new StaticPagedList<SerialNumberRule>(
                result,
                pageNumber,
                pageSize,
                totalCount);
        }

        public bool Exists(SerialNumberRule item)
        {
            IQueryable<SerialNumberRule> entities = Table;
            entities = entities.Where(x => x.RuleName == item.RuleName);
            if(item.Id != Guid.Empty)
            {
                entities = entities.Where(x => x.Id != item.Id);
            }
            return entities.Any();
        }
    }
}
