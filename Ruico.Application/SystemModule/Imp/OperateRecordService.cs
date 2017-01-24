using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using Ruico.Domain.SystemModule.Entities;
using Ruico.Domain.SystemModule.Repositories;
using Ruico.Dto.System;
using Ruico.Dto.System.Converters;
using Ruico.Infrastructure.Entity;

namespace Ruico.Application.SystemModule.Imp
{
    public class OperateRecordService : IOperateRecordService
    {
        IOperateRecordRepository _Repository;

        #region Constructors

        public OperateRecordService(IOperateRecordRepository repository)                               
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            _Repository = repository;
        }

        #endregion

        public OperateRecordDTO Add(OperateRecordDTO operateRecordDTO, bool commit = true)
        {
            var record = operateRecordDTO.ToModel();
            record.Id = IdentityGenerator.NewSequentialGuid();
            if (record.OperateTime == DateTime.MinValue)
            {
                record.OperateTime = DateTime.UtcNow;
            }

            if (record.Message != null && record.Message.Length > 150)
            {
                var message = record.Message;
                record.Message = record.Message.Substring(0, 147) + "...";
                // 把message添加到Extend表
                var extend = new OperateRecordExtend()
                {
                    Id = IdentityGenerator.NewSequentialGuid(),
                    LongMessage = message
                };
                record.ExtendId = extend.Id;
                _Repository.AddExtend(extend);
            }

            _Repository.Add(record);

            if (commit)
            {
                //commit the unit of work
                _Repository.UnitOfWork.Commit();
            }

            return record.ToDto();
        }

        public IPagedList<OperateRecordDTO> FindBy(string sn, int pageNumber, int pageSize)
        {
            var list = _Repository.FindBy(sn, pageNumber, pageSize);

            if (list.Count == 0)
            {
                list = this.FindArchiveBy(sn, pageNumber, pageSize);
            }

            // 获取操作明细长内容
            var extIds = list.Where(x => x.ExtendId.HasValue)
                .Select(x => x.ExtendId.Value).ToList();
            if (extIds.Any())
            {
                var extRecords = this.FindExtendBy(extIds);
                foreach (var item in list.Where(x => x.ExtendId.HasValue))
                {
                    var ext = extRecords.FirstOrDefault(x => x.Id == item.ExtendId);
                    if (ext != null)
                    {
                        item.Message = ext.LongMessage;
                    }
                }
            }

            return new StaticPagedList<OperateRecordDTO>(
                list.ToList().Select(x => x.ToDto()),
                pageNumber,
                pageSize,
                list.TotalItemCount);
        }

        private IPagedList<OperateRecord> FindArchiveBy(string sn, int pageNumber, int pageSize)
        {
            var list = _Repository.FindArchiveBy(sn, pageNumber, pageSize);

            return new StaticPagedList<OperateRecord>(
                list.ToList().Select(x => x.ToDto().ToModel()),
                pageNumber,
                pageSize,
                list.TotalItemCount);
        }

        private IList<OperateRecordExtendDTO> FindExtendBy(List<Guid> ids)
        {
            var list = _Repository.FindExtendBy(ids);

            return list.Select(x => x.ToDto()).ToList();
        }
    }
}
