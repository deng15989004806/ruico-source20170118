using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using Ruico.Domain.SystemModule.Entities;
using Ruico.Infrastructure.Repository;

namespace Ruico.Domain.SystemModule.Repositories
{
    public interface ISerialNumberRepository : IRepository<SerialNumber>
    {
        int GetSerialNumber(string prefix, string dateNumber, int increase);

        IPagedList<SerialNumber> FindBy(int pageNumber, int pageSize);

        void Remove(long id);
    }
}
