using System;
using System.Collections.Generic;
using PagedList;
using Ruico.Dto.Base;

namespace Ruico.Application.BaseModule
{
    public interface ICompanyService
    {
        CompanyDTO Add(CompanyDTO companyDTO);

        void Update(CompanyDTO companyDTO);

        void Remove(Guid id);

        CompanyDTO FindBy(Guid id);

        IPagedList<CompanyDTO> FindBy(string name, Guid? categoryId, int pageNumber, int pageSize);

        IList<CategoryDTO> GetCompanyCategories();
    }
}
