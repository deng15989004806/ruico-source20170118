using System;
using System.Collections.Generic;

namespace Ruico.Dto.Hr
{
    public class DepartmentDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int DepartmentId { get; set; }

        public int SortOrder { get; set; }

        public DateTime Created { get; set; }

        public int ParentId { get; set; }

        public int Depth { get; set; }
    }
}
