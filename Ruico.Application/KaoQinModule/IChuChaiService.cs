using System;
using System.Collections.Generic;
using System.Data;
using PagedList;
using Ruico.Dto.KaoQin;
using Ruico.Dto.UserSystem;

namespace Ruico.Application.KaoQinModule
{
    public interface IChuChaiService
    {
        ChuChaiDTO Add(ChuChaiDTO item, UserDTO operatorDTO = null);

        void Approve(ChuChaiDTO itemDto, UserDTO operatorDTO = null);

        void Cancel(Guid id, UserDTO operatorDTO = null);

        void Remove(Guid id);

        ChuChaiDTO FindBy(Guid id);

        IPagedList<ChuChaiDTO> FindBy(KaoQinConditionDTO condition, int pageNumber, int pageSize);

        DataTable GetExportTable(List<ChuChaiDTO> items);
    }
}
