using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ruico.Infrastructure.Entity;

namespace Ruico.Domain.SystemModule.Entities
{
    [Table("system.SerialNumber")]
    public class SerialNumber : EntityBase
    {
        public new long Id { get; set; }

        [MaxLength(10)]
        public string Prefix { get; set; }

        [MaxLength(8)]
        public string DateNumber { get; set; }

        public int Sequence { get; set; }

        public DateTime Created { get; set; }
    }
}
