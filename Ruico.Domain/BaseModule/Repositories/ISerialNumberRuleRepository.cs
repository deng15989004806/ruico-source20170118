using PagedList;
using Ruico.Domain.BaseModule.Entities;
using Ruico.Infrastructure.Repository;

namespace Ruico.Domain.BaseModule.Repositories
{
    public interface ISerialNumberRuleRepository : IRepository<SerialNumberRule>
    {
        IPagedList<SerialNumberRule> FindBy(string name, int pageNumber, int pageSize);

        bool Exists(SerialNumberRule item);
    }
}
