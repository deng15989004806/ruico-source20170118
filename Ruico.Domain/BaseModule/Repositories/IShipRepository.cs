using System;
using PagedList;
using Ruico.Domain.BaseModule.Entities;
using Ruico.Infrastructure.Repository;

namespace Ruico.Domain.BaseModule.Repositories
{
    public interface IShipRepository : IRepository<Ship>
    {
        IPagedList<Ship> FindBy(string name, int pageNumber, int pageSize);

        bool Exists(Ship item);
    }
}
