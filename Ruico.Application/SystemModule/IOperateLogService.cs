using Ruico.Dto.UserSystem;

namespace Ruico.Application.SystemModule
{
    /// <summary>
    /// 对表现层公开操作日志的方法
    /// </summary>
    public interface IOperateLogService
    {
        void RecordOperation(string sn, string operation, string message,
            UserDTO user = null, bool commit = false);
    }
}
