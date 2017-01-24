using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ruico.Infrastructure.Entity;

namespace Ruico.Domain.UserSystemModule.Entities
{
    [Table("auth.Permission")]
    public class Permission : EntityBase
    {
        public override Guid Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string Code { get; set; }

        [MaxLength(150)]
        public string ActionUrl { get; set; }

        public int SortOrder { get; set; }

        public DateTime Created { get; set; }

        public virtual Menu Menu { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
