using System;
using PagedList;
using Ruico.Dto.Base;

namespace Ruico.Application.BaseModule
{
    public interface ICategoryService
    {
        CategoryDTO Add(CategoryDTO categoryDTO);

        void Update(CategoryDTO categoryDTO);

        void Remove(Guid id);

        CategoryDTO FindBy(Guid id);

        CategoryDTO FindBySn(string sn);

        CategoryDTO FindByName(string name);

        IPagedList<CategoryDTO> FindBy(string name, int depth, Guid? parentId, int pageNumber, int pageSize);
    }
}
