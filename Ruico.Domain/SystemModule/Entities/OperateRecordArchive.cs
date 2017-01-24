using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ruico.Infrastructure.Entity;

namespace Ruico.Domain.SystemModule.Entities
{
    [Table("system.OperateRecordArchive")]
    public class OperateRecordArchive : EntityBase
    {
        public override Guid Id { get; set; }

        [MaxLength(36)]
        [Required]
        [Index]
        public string Sn { get; set; }

        public Guid? UserId { get; set; }

        [MaxLength(20)]
        public string OperatorName { get; set; }

        [MaxLength(20)]
        public string Operation { get; set; }

        [MaxLength(150)]
        public string Message { get; set; }

        public DateTime OperateTime { get; set; }

        [MaxLength(20)]
        public string Ip { get; set; }

        public Guid? ExtendId { get; set; }
    }
}
