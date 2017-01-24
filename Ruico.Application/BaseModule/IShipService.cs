using System;
using PagedList;
using Ruico.Dto.Base;

namespace Ruico.Application.BaseModule
{
    public interface IShipService
    {
        ShipDTO Add(ShipDTO shipDTO);

        void Update(ShipDTO shipDTO);

        void Remove(Guid id);

        ShipDTO FindBy(Guid id);

        IPagedList<ShipDTO> FindBy(string name, int pageNumber, int pageSize);
    }
}
