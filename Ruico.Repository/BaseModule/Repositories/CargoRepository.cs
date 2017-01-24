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
    public class CargoRepository : EfRepository<Cargo>, ICargoRepository
    {
        public CargoRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public IPagedList<Cargo> FindBy(string name, Guid? categoryId, int pageNumber, int pageSize)
        {
            IQueryable<Cargo> entities = Table; 
            
            if (name.NotNullOrBlank())
            {
                entities =
                    entities.Where(x => x.Name.Contains(name));
            }
            if (categoryId.HasValue)
            {
                entities =
                    entities.Where(x => x.Category != null && x.Category.Id == categoryId);
            }

            var totalCountQuery = entities.FutureCount();
            var resultQuery = entities
                .OrderByDescending(x => x.Created)
                .ThenByDescending(x => x.Sn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Future();

            var totalCount = totalCountQuery.Value;
            var result = resultQuery.ToList();

            return new StaticPagedList<Cargo>(
                result,
                pageNumber,
                pageSize,
                totalCount);
        }

        public bool Exists(Cargo item)
        {
            IQueryable<Cargo> entities = Table;
            entities = entities.Where(x => x.Name == item.Name);
            if(item.Id != Guid.Empty)
            {
                entities = entities.Where(x => x.Id != item.Id);
            }
            return entities.Any();
        }
    }
}
