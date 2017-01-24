using System;
using PagedList;
using Ruico.Domain.WeixinModule.Entities;
using Ruico.Infrastructure.Repository;

namespace Ruico.Domain.WeixinModule.Repositories
{
    public interface IAppMenuRepository : IRepository<AppMenu>
    {
        IPagedList<AppMenu> FindBy(int? appId, string name, int pageNumber, int pageSize);

        bool Exists(AppMenu item);
    }
}
