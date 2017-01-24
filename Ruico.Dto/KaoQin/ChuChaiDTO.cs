using System;

namespace Ruico.Dto.KaoQin
{
    public class ChuChaiDTO
    {
        public Guid Id { get; set; }
        
        public string UserId { get; set; }
        
        public string Name { get; set; }
        
        public int DepartmentId { get; set; }
        
        public string Department { get; set; }
        
        public string Position { get; set; }
        
        public DateTime OutTime { get; set; }
        
        public DateTime InTime { get; set; }
        
        public string OutPlace { get; set; }
        
        public string OutReason { get; set; }

        public string AppointPerson { get; set; }

        public string DepartmentOpinion { get; set; }
        
        public string DepartmentOpinionApproverId { get; set; }
        
        public string GeneralManagerOfficeOpinion { get; set; }
        
        public string GeneralManagerOfficeOpinionApproverId { get; set; }
        
        public string CompanyLeaderOpinion { get; set; }
        
        public string CompanyLeaderOpinionApproverId { get; set; }

        public string Status { get; set; }

        public DateTime Created { get; set; }
        
        public DateTime Canceled { get; set; }

        public DateTime Approved { get; set; }
    }
}
