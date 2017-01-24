using System;
using System.Collections.Generic;
using System.Data;
using PagedList;
using Ruico.Dto.KaoQin;
using Ruico.Dto.UserSystem;

namespace Ruico.Application.KaoQinModule
{
    public interface IXiuJiaService
    {
        XiuJiaDTO Add(XiuJiaDTO item, UserDTO operatorDTO = null);

        void Approve(XiuJiaDTO itemDto, UserDTO operatorDTO = null);

        void Cancel(Guid id, UserDTO operatorDTO = null);

        void Remove(Guid id);

        XiuJiaDTO FindBy(Guid id);

        IPagedList<XiuJiaDTO> FindBy(KaoQinConditionDTO condition, int pageNumber, int pageSize);

        DataTable GetExportTable(List<XiuJiaDTO> items);
    }
}
