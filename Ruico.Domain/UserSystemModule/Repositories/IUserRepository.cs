using PagedList;
using Ruico.Domain.UserSystemModule.Entities;
using Ruico.Infrastructure.Repository;

namespace Ruico.Domain.UserSystemModule.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        IPagedList<User> FindBy(string name, int pageNumber, int pageSize);

        bool ExistsLoginName(User item);

        bool ExistsEmail(User item);

        bool ExistsName(User item);
    }
}
