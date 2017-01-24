using System;
using System.Collections.Generic;
using PagedList;
using Ruico.Dto.UserSystem;

namespace Ruico.Application.UserSystemModule
{
    public interface IMenuService
    {
        MenuDTO Add(MenuDTO menuDTO);

        void Update(MenuDTO menuDTO);

        void Remove(Guid id);

        void UpdatePermission(MenuDTO menuDTO);

        MenuDTO FindBy(Guid id);

        IPagedList<MenuDTO> FindBy(Guid moduleId, string name, int pageNumber, int pageSize);

        List<MenuDTO> FindByModule(Guid moduleId);
    }
}
