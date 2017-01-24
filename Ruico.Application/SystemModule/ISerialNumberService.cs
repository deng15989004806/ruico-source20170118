using PagedList;
using Ruico.Dto.System;

namespace Ruico.Application.SystemModule
{
    public interface ISerialNumberService
    {
        int GetSerialNumber(string prefix, string dateNumber, int increase);

        IPagedList<SerialNumberDTO> FindBy(int pageNumber, int pageSize);

        void Remove(long id);
    }
}
