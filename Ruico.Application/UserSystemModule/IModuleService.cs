using System;
using System.Collections.Generic;
using PagedList;
using Ruico.Dto.UserSystem;

namespace Ruico.Application.UserSystemModule
{
    public interface IModuleService
    {
        ModuleDTO Add(ModuleDTO menuDTO);

        void Update(ModuleDTO menuDTO);

        void Remove(Guid id);

        ModuleDTO FindBy(Guid id);

        IPagedList<ModuleDTO> FindBy(string name, int pageNumber, int pageSize);

        List<ModuleDTO> ListAll();
    }
}
