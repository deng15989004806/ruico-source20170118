using Ruico.Dto.UserSystem;

namespace Ruico.Application.SystemModule.Imp
{
    /// <summary>
    /// 对表现层公开操作日志的方法
    /// </summary>
    public class OperateLogService : IOperateLogService
    {
        public void RecordOperation(string sn, string operation, string message,
            UserDTO user = null, bool commit = false)
        {
            OperateRecorder.RecordOperation(sn, operation, message, user, commit);
        }
    }
}
