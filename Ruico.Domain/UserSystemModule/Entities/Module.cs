using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ruico.Infrastructure.Entity;

namespace Ruico.Domain.UserSystemModule.Entities
{
    [Table("auth.Module")]
    public class Module : EntityBase
    {
        public override Guid Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public int SortOrder { get; set; }

        public DateTime Created { get; set; }
    }
}
