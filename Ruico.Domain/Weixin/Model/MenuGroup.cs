using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruico.Domain.Weixin.Model
{
    public class MenuGroup
    {
        public MenuGroup()
        {
            Childs = new List<Menu>();
        }

        public Menu Menu { get; set; }

        public List<Menu> Childs { get; set; }
    }
}
