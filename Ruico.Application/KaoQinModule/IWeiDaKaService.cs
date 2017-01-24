using System;
using System.Collections.Generic;
using System.Data;
using PagedList;
using Ruico.Dto.KaoQin;
using Ruico.Dto.UserSystem;

namespace Ruico.Application.KaoQinModule
{
    public interface IWeiDaKaService
    {
        WeiDaKaDTO Add(WeiDaKaDTO item, UserDTO operatorDTO = null);

        void Approve(WeiDaKaDTO itemDto, UserDTO operatorDTO = null);

        void Cancel(Guid id, UserDTO operatorDTO = null);

        void Remove(Guid id);

        WeiDaKaDTO FindBy(Guid id);

        IPagedList<WeiDaKaDTO> FindBy(KaoQinConditionDTO condition, int pageNumber, int pageSize);

        DataTable GetExportTable(List<WeiDaKaDTO> items);
    }
}
