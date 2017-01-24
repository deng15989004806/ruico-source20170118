using System;
using PagedList;
using Ruico.Domain.BaseModule.Entities;
using Ruico.Infrastructure.Repository;

namespace Ruico.Domain.BaseModule.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        IPagedList<Category> FindBy(string name, int depth, Guid? parentId, int pageNumber, int pageSize);

        bool Exists(Category item);
    }
}
