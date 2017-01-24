using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Ruico.Infrastructure.Entity;

namespace Ruico.Domain.HrModule.Entities
{
    [Table("hr.Department")]
    public class Department : EntityBase
    {
        public override Guid Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public int DepartmentId { get; set; }

        public int SortOrder { get; set; }

        public DateTime Created { get; set; }

        public int ParentId { get; set; }

        public virtual ICollection<Member> Members { get; set; }
    }
}
