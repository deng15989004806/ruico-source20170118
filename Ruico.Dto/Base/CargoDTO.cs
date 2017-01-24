using System;
using System.Collections.Generic;

namespace Ruico.Dto.Base
{
    public class CargoDTO
    {
        public Guid Id { get; set; }

        public string Sn { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        public Guid CreatorId { get; set; }

        public CategoryDTO Category { get; set; }
    }
}
