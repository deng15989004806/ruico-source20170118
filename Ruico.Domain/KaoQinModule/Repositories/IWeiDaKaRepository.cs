using System;
using System.Collections.Generic;
using PagedList;
using Ruico.Domain.KaoQinModule.Entities;
using Ruico.Infrastructure.Repository;

namespace Ruico.Domain.KaoQinModule.Repositories
{
    public interface IWeiDaKaRepository : IRepository<WeiDaKa>
    {
        IPagedList<WeiDaKa> FindBy(KaoQinCondition condition, int pageNumber, int pageSize);

        bool Exists(WeiDaKa item);
    }
}
