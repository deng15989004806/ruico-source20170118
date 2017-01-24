namespace Ruico.Application.Exceptions
{
    /// <summary>
    /// 数据已存在
    /// </summary>
    public class DataExistsException : DefinedException
    {
        public DataExistsException(string message, params object[] paramObjects)
            : base(message, paramObjects)
        {
            
        }
    }
}
