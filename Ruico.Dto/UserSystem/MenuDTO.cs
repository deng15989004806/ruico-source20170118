using System;
using System.Collections.Generic;
using Ruico.Dto.Common;

namespace Ruico.Dto.UserSystem
{
    public class MenuDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Url { get; set; }

        public int SortOrder { get; set; }

        public int Depth { get; set; }

        public DateTime Created { get; set; }

        public ModuleDTO Module { get; set; }

        public MenuDTO Parent { get; set; }

        public virtual ICollection<PermissionDTO> Permissions { get; set; }
    }
}
