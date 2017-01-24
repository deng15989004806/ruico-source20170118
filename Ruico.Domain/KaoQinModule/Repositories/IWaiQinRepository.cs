using System;
using System.Collections.Generic;
using PagedList;
using Ruico.Domain.KaoQinModule.Entities;
using Ruico.Infrastructure.Repository;

namespace Ruico.Domain.KaoQinModule.Repositories
{
    public interface IWaiQinRepository : IRepository<WaiQin>
    {
        IPagedList<WaiQin> FindBy(KaoQinCondition condition, int pageNumber, int pageSize);

        bool Exists(WaiQin item);
    }
}
