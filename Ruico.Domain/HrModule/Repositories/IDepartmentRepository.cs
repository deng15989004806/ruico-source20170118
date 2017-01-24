using System;
using PagedList;
using Ruico.Domain.HrModule.Entities;
using Ruico.Infrastructure.Repository;

namespace Ruico.Domain.HrModule.Repositories
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        IPagedList<Department> FindBy(int? parentId, string name, int pageNumber, int pageSize);

        bool Exists(Department item);
    }
}
