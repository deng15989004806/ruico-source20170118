using System;
using System.Collections.Generic;

namespace Ruico.Dto.KaoQin
{
    public class KaoQinConditionDTO
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
