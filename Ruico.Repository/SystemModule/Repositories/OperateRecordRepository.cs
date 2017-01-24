using System;
using System.Collections.Generic;
using System.Linq;
using EntityFramework.Extensions;
using PagedList;
using Ruico.Domain.SystemModule.Entities;
using Ruico.Domain.SystemModule.Repositories;
using Ruico.Infrastructure.UnitOfWork;
using Ruico.Infrastructure.Utility.Helper;

namespace Ruico.Repository.SystemModule.Repositories
{
    public class OperateRecordRepository : EfRepository<OperateRecord>, IOperateRecordRepository
    {
        public OperateRecordRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public IPagedList<OperateRecord> FindBy(string sn, int pageNumber, int pageSize)
        {
            IQueryable<OperateRecord> entities = Table;

            if (sn.NotNullOrBlank())
            {
                entities =
                    entities.Where(x => x.Sn == sn);
            }

            var totalCountQuery = entities.FutureCount();
            var resultQuery = entities
                .OrderByDescending(x => x.OperateTime)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Future();

            var totalCount = totalCountQuery.Value;
            var result = resultQuery.ToList();

            return new StaticPagedList<OperateRecord>(
                result,
                pageNumber,
                pageSize,
                totalCount);
        }

        public IPagedList<OperateRecordArchive> FindArchiveBy(string sn, int pageNumber, int pageSize)
        {
            var unitOfWork = (this.UnitOfWork as RuicoUnitOfWork);
            IQueryable<OperateRecordArchive> entities = unitOfWork.OperateRecordArchives;

            if (sn.NotNullOrBlank())
            {
                entities =
                    entities.Where(x => x.Sn == sn);
            }

            var totalCountQuery = entities.FutureCount();
            var resultQuery = entities
                .OrderByDescending(x => x.OperateTime)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Future();

            var totalCount = totalCountQuery.Value;
            var result = resultQuery.ToList();

            return new StaticPagedList<OperateRecordArchive>(
                result,
                pageNumber,
                pageSize,
                totalCount);
        }

        public IList<OperateRecordExtend> FindExtendBy(List<Guid> ids)
        {
            var unitOfWork = (this.UnitOfWork as RuicoUnitOfWork);
            IQueryable<OperateRecordExtend> entities = unitOfWork.OperateRecordExtends;

            return entities.Where(x => ids.Contains(x.Id)).ToList();
        }

        public OperateRecordExtend AddExtend(OperateRecordExtend item)
        {
            var unitOfWork = (this.UnitOfWork as RuicoUnitOfWork);
            unitOfWork.OperateRecordExtends.Add(item);

            return item;
        }
    }
}
