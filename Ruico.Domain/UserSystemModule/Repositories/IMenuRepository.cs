using System;
using PagedList;
using Ruico.Domain.UserSystemModule.Entities;
using Ruico.Infrastructure.Repository;

namespace Ruico.Domain.UserSystemModule.Repositories
{
    public interface IMenuRepository : IRepository<Menu>
    {
        IPagedList<Menu> FindBy(Guid moduleId, string name, int pageNumber, int pageSize);

        bool Exists(Menu item);
    }
}
