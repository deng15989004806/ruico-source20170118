using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruico.Domain.Weixin.Model
{
    public class Menu
    {
        public Menu(string type, string name, string key, string url = "")
        {
            this.Type = type;
            this.Name = name;
            this.Key = key;
            this.Url = url;
        }

        public Menu(string name)
        {
            this.Name = name;
        }

        public string Type { get; set; }

        public string Name { get; set; }

        public string Key { get; set; }

        public string Url { get; set; }
    }
}
