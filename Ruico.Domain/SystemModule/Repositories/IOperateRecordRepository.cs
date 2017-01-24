using System;
using System.Collections.Generic;
using PagedList;
using Ruico.Domain.SystemModule.Entities;
using Ruico.Infrastructure.Repository;

namespace Ruico.Domain.SystemModule.Repositories
{
    public interface IOperateRecordRepository : IRepository<OperateRecord>
    {
        IPagedList<OperateRecord> FindBy(string sn, int pageNumber, int pageSize);

        IPagedList<OperateRecordArchive> FindArchiveBy(string sn, int pageNumber, int pageSize);

        IList<OperateRecordExtend> FindExtendBy(List<Guid> ids);

        OperateRecordExtend AddExtend(OperateRecordExtend item);
    }
}
