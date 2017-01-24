using System;
using System.Collections.Generic;

namespace Ruico.Dto.Hr
{
    public class MemberDTO
    {
        public Guid Id { get; set; }
        
        public string Userid { get; set; }
    
        public string Name { get; set; }

        public string WeixinId { get; set; }
        
        public string Position { get; set; }
        
        public int Gender { get; set; }

        public string Mobile { get; set; }
        
        public string Email { get; set; }
        
        public int Status { get; set; }
        
        public string Avatar { get; set; }
        
        public int Enable { get; set; }

        public DateTime Created { get; set; }

        public List<DepartmentDTO> Departments { get; set; }
    }
}
