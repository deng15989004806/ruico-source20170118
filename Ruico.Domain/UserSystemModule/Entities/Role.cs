using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ruico.Infrastructure.Entity;

namespace Ruico.Domain.UserSystemModule.Entities
{
    [Table("auth.Role")]
    public class Role : EntityBase
    {
        public override Guid Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string Description { get; set; }

        public int SortOrder { get; set; }

        public DateTime Created { get; set; }

        public virtual RoleGroup RoleGroup { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
