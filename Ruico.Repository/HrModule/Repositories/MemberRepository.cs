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
    public class MemberRepository : EfRepository<Member>, IMemberRepository
    {
        public MemberRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public IPagedList<Member> FindBy(string name, int pageNumber, int pageSize)
        {
            IQueryable<Member> entities = Table;

            if (name.NotNullOrBlank())
            {
                entities =
                    entities.Where(x => x.Name.Contains(name));

            }

            var totalCountQuery = entities.FutureCount();
            var resultQuery = entities
                .OrderBy(x => x.Created)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Future();

            var totalCount = totalCountQuery.Value;
            var result = resultQuery.ToList();

            return new StaticPagedList<Member>(
                result,
                pageNumber,
                pageSize,
                totalCount);
        }

        public Member FindByUserId(string userId)
        {
            return Table.FirstOrDefault(x => x.Userid == userId);
        }

        public bool Exists(Member item)
        {
            IQueryable<Member> entities = Table;
            entities = entities.Where(x => 
                (x.Userid != null && x.Userid!="" && x.Userid == item.Userid)
                || (x.WeixinId != null && x.WeixinId != "" && x.WeixinId == item.WeixinId)
                || (x.Email != null && x.Email != "" && x.Email == item.Email));
            if(item.Id != Guid.Empty)
            {
                entities = entities.Where(x => x.Id != item.Id);
            }
            return entities.Any();
        }
    }
}
