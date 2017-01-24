using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruico.Domain.Weixin.Model
{
    public class Department
    {
        public Department(int id, string name, int parentId = 1, int order = 0, int depth = 0)
        {
            this.Id = id;
            this.Name = name;
            this.ParentId = parentId;
            this.Order = order;
            this.Depth = depth;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int ParentId { get; set; }

        public int Order { get; set; }

        public int Depth { get; set; }
    }
}
