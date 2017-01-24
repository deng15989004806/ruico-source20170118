using System;
using PagedList;
using Ruico.Domain.BaseModule.Entities;
using Ruico.Infrastructure.Repository;

namespace Ruico.Domain.BaseModule.Repositories
{
    public interface ICargoRepository : IRepository<Cargo>
    {
        IPagedList<Cargo> FindBy(string name, Guid? categoryId, int pageNumber, int pageSize);

        bool Exists(Cargo item);
    }
}
