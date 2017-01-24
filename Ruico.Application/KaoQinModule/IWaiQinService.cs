using System;
using System.Collections.Generic;
using System.Data;
using PagedList;
using Ruico.Dto.KaoQin;
using Ruico.Dto.UserSystem;

namespace Ruico.Application.KaoQinModule
{
    public interface IWaiQinService
    {
        WaiQinDTO Add(WaiQinDTO item, UserDTO operatorDTO = null);

        void Approve(WaiQinDTO itemDto, UserDTO operatorDTO = null);

        void Cancel(Guid id, UserDTO operatorDTO = null);

        void Remove(Guid id);

        WaiQinDTO FindBy(Guid id);

        IPagedList<WaiQinDTO> FindBy(KaoQinConditionDTO condition, int pageNumber, int pageSize);

        DataTable GetExportTable(List<WaiQinDTO> items);
    }
}
