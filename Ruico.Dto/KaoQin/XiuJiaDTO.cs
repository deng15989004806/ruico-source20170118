using System;

namespace Ruico.Dto.KaoQin
{
    public class XiuJiaDTO
    {
        public Guid Id { get; set; }
        
        public string UserId { get; set; }
        
        public string Name { get; set; }
        
        public int DepartmentId { get; set; }
        
        public string Department { get; set; }
        
        public string Position { get; set; }
        
        public DateTime InputTime { get; set; }
        
        public DateTime ActionStartTime { get; set; }
        
        public DateTime ActionEndTime { get; set; }
        
        public decimal ActionDays { get; set; }
        
        public decimal ActionHours { get; set; }
        
        public string Type { get; set; }
        
        public string Reason { get; set; }
        
        public string DepartmentSupervisorOpinion { get; set; }
        
        public string DepartmentSupervisorOpinionApproverId { get; set; }
        
        public string DepartmentManagerOpinion { get; set; }
        
        public string DepartmentManagerOpinionApproverId { get; set; }
        
        public string CompanyLeaderOpinion { get; set; }
        
        public string CompanyLeaderOpinionApproverId { get; set; }

        public string Status { get; set; }

        public DateTime Created { get; set; }

        public DateTime Canceled { get; set; }

        public DateTime Approved { get; set; }
    }
}
