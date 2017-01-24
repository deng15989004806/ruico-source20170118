using System;
using System.ComponentModel.DataAnnotations.Schema;
using Ruico.Infrastructure.Entity;

namespace Ruico.Domain.SystemModule.Entities
{
    [Table("system.OperateRecordExtend")]
    public class OperateRecordExtend : EntityBase
    {
        public override Guid Id { get; set; }

        public string LongMessage { get; set; }
    }
}
