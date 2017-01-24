using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruico.Dto.System
{
    public class SerialNumberDTO
    {
        public long Id { get; set; }

        public string Prefix { get; set; }

        public string DateNumber { get; set; }

        public int Sequence { get; set; }

        public DateTime Created { get; set; }
    }
}
