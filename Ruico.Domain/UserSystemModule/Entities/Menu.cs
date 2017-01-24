using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ruico.Infrastructure.Entity;

namespace Ruico.Domain.UserSystemModule.Entities
{
    [Table("auth.Menu")]
    public class Menu : EntityBase
    {
        public override Guid Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(150)]
        public string Code { get; set; }

        [MaxLength(150)]
        public string Url { get; set; }

        public int SortOrder { get; set; }

        public int Depth { get; set; }

        public DateTime Created { get; set; }

        public virtual Module Module { get; set; }

        public virtual Menu Parent { get; set; }

        public virtual ICollection<Menu> Children { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
