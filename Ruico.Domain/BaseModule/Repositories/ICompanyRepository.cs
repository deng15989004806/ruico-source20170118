using System;
using System.Collections.Generic;
using PagedList;
using Ruico.Domain.BaseModule.Entities;
using Ruico.Infrastructure.Repository;

namespace Ruico.Domain.BaseModule.Repositories
{
    public interface ICompanyRepository : IRepository<Company>
    {
        IPagedList<Company> FindBy(string name, Guid? categoryId, int pageNumber, int pageSize);

        bool Exists(Company item);
    }
}
