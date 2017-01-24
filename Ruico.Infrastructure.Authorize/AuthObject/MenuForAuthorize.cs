using System;

namespace Ruico.Infrastructure.Authorize.AuthObject
{
    public class MenuForAuthorize
    {
        public Guid ModuleId { get; set; }

        public string ModuleName { get; set; }

        public Guid MenuId { get; set; }

        public string MenuName { get; set; }

        public string MenuUrl { get; set; }

        public int MenuDepth { get; set; }

        public int MenuSortOrder { get; set; }

        public Guid ParentMenuId { get; set; }
    }
}
