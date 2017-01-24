using PagedList;
using Ruico.Dto.System;

namespace Ruico.Application.SystemModule
{
    public interface IOperateRecordService
    {
        OperateRecordDTO Add(OperateRecordDTO operateRecordDTO, bool commit = true);

        IPagedList<OperateRecordDTO> FindBy(string sn, int pageNumber, int pageSize);
    }
}
