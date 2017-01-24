using System;
using System.Collections.Generic;

namespace Ruico.Domain.KaoQinModule.Entities
{
    public class KaoQinCondition
    {
        public string UserId { get; set; }

        public List<int> DepartmentIds { get; set; }

        public List<string> Statuses { get; set; }

        public DateTime? CreatedStartTime { get; set; }

        public DateTime? CreatedEndTime { get; set; }

        public string ExcludeUserId { get; set; }

        public List<string> ExcludePositions { get; set; }
    }
}
