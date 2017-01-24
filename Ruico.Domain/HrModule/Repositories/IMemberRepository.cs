using System;
using PagedList;
using Ruico.Domain.HrModule.Entities;
using Ruico.Infrastructure.Repository;

namespace Ruico.Domain.HrModule.Repositories
{
    public interface IMemberRepository : IRepository<Member>
    {
        IPagedList<Member> FindBy(string name, int pageNumber, int pageSize);

        Member FindByUserId(string userId);

        bool Exists(Member item);
    }
}
