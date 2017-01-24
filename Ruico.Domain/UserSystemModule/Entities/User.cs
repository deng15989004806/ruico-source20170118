using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ruico.Infrastructure.Entity;

namespace Ruico.Domain.UserSystemModule.Entities
{
    [Table("auth.User")]
    public class User : EntityBase
    {
        public override Guid Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string LoginName { get; set; }

        [MaxLength(50)]
        public string LoginPwd { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        public DateTime Created { get; set; }

        [MaxLength(50)]
        public string LastLoginToken { get; set; }

        public DateTime LastLogin { get; set; }

        public virtual ICollection<Permission> Permissions { get; set; }

        public virtual ICollection<RoleGroup> Groups { get; set; }

    }
}
