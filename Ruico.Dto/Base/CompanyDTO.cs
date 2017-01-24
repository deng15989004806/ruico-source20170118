using System;
using System.Collections.Generic;

namespace Ruico.Dto.Base
{
    public class CompanyDTO
    {
        public Guid Id { get; set; }

        public string Sn { get; set; }

        public string Name { get; set; }

        public string Contact { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Mobile { get; set; }

        public string Address { get; set; }

        public string Postcode { get; set; }

        public string Email { get; set; }

        public DateTime Created { get; set; }

        public Guid CreatorId { get; set; }

        public CategoryDTO Category { get; set; }
    }
}
