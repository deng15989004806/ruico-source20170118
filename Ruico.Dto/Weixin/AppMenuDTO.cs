using System;
using System.Collections.Generic;

namespace Ruico.Dto.Weixin
{
    public class AppMenuDTO
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string Key { get; set; }
        
        public string Url { get; set; }

        public int SortOrder { get; set; }

        public int AppId { get; set; }

        public DateTime Created { get; set; }

        public AppMenuDTO Parent { get; set; }
    }
}
