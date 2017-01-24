namespace Ruico.Application.Exceptions
{
    /// <summary>
    /// 数据不存在
    /// </summary>
    public class DataNotFoundException : DefinedException
    {
        public DataNotFoundException(string message, params object[] paramObjects)
            : base(message, paramObjects)
        {
            
        }
    }
}
