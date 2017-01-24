using System;
using System.Collections.Generic;
using PagedList;
using Ruico.Dto.Base;

namespace Ruico.Application.BaseModule
{
    public interface ICargoService
    {
        CargoDTO Add(CargoDTO cargoDTO);

        void Update(CargoDTO cargoDTO);

        void Remove(Guid id);

        CargoDTO FindBy(Guid id);

        IPagedList<CargoDTO> FindBy(string name, Guid? categoryId, int pageNumber, int pageSize);

        IList<CategoryDTO> GetCargoCategories();
    }
}
