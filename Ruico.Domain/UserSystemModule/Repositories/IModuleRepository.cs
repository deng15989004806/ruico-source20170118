using PagedList;
using Ruico.Domain.UserSystemModule.Entities;
using Ruico.Infrastructure.Repository;

namespace Ruico.Domain.UserSystemModule.Repositories
{
    public interface IModuleRepository : IRepository<Module>
    {
        IPagedList<Module> FindBy(string name, int pageNumber, int pageSize);

        bool Exists(Module item);
    }
}
