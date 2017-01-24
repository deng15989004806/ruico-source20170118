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
    public class UserRepository : EfRepository<User>, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public IPagedList<User> FindBy(string name, int pageNumber, int pageSize)
        {
            IQueryable<User> entities = Table;

            if (name.NotNullOrBlank())
            {
                entities =
                    entities.Where(x => x.Name.Contains(name));
            }

            var totalCountQuery = entities.FutureCount();
            var resultQuery = entities
                .OrderByDescending(x => x.Created)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Future();

            var totalCount = totalCountQuery.Value;
            var result = resultQuery.ToList();

            return new StaticPagedList<User>(
                result,
                pageNumber,
                pageSize,
                totalCount);
        }

        public bool ExistsLoginName(User item)
        {
            IQueryable<User> entities = Table;
            entities = entities.Where(x => x.LoginName == item.LoginName);
            if (item.Id != Guid.Empty)
            {
                entities = entities.Where(x => x.Id != item.Id);
            }
            return entities.Any();
        }

        public bool ExistsEmail(User item)
        {
            IQueryable<User> entities = Table;
            entities = entities.Where(x => x.Email == item.Email);
            if (item.Id != Guid.Empty)
            {
                entities = entities.Where(x => x.Id != item.Id);
            }
            return entities.Any();
        }

        public bool ExistsName(User item)
        {
            IQueryable<User> entities = Table;
            entities = entities.Where(x => x.Name == item.Name);
            if (item.Id != Guid.Empty)
            {
                entities = entities.Where(x => x.Id != item.Id);
            }
            return entities.Any();
        }
    }
}
