using System;

namespace Ruico.Dto.System
{
    public class OperateRecordDTO
    {
        public Guid Id { get; set; }

        public string Sn { get; set; }

        public Guid? UserId { get; set; }

        public string OperatorName { get; set; }

        public string Operation { get; set; }

        public string Message { get; set; }

        public DateTime OperateTime { get; set; }

        public string Ip { get; set; }
    }
}
