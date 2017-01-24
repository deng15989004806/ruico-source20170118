using System;

namespace Ruico.Dto.KaoQin
{
    public class WeiDaKaDTO
    {
        public Guid Id { get; set; }
        
        public string UserId { get; set; }
        
        public string Name { get; set; }
        
        public int DepartmentId { get; set; }
        
        public string Department { get; set; }
        
        public string Position { get; set; }
        
        public DateTime InputTime { get; set; }
        
        public DateTime ActionTime { get; set; }
        
        public string Type { get; set; }
        
        public string Reason { get; set; }
        
        public string DepartmentOrCompanyOpinion { get; set; }
        
        public string DepartmentOrCompanyOpinionApproverId { get; set; }

        public string Status { get; set; }

        public DateTime Created { get; set; }

        public DateTime Canceled { get; set; }

        public DateTime Approved { get; set; }
    }
}
