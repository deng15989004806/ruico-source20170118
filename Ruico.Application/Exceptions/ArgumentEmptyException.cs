namespace Ruico.Application.Exceptions
{
    /// <summary>
    /// 参数不能为空
    /// </summary>
    public class ArgumentEmptyException : DefinedException
    {
        public ArgumentEmptyException(string message)
            : base(message)
        {
            
        }
    }
}
