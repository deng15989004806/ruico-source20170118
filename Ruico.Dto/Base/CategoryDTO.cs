using System;
using System.Collections.Generic;

namespace Ruico.Dto.Base
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }

        public string Sn { get; set; }

        public string Name { get; set; }

        public int SortOrder { get; set; }

        public int Depth { get; set; }

        public string ChildSnRulePrefix { get; set; }

        public int ChildSnRuleNumberLength { get; set; }

        public DateTime Created { get; set; }

        public Guid CreatorId { get; set; }

        public CategoryDTO Parent { get; set; }
    }
}
