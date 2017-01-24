using System;

namespace Ruico.Application.Exceptions
{
    public class DefinedException: Exception
    {
        protected object[] ParamObjects;

        public DefinedException(string message)
            : base(message)
        {
            
        }

        public DefinedException(string message, params object[] paramObjects)
            : base(message)
        {
            ParamObjects = paramObjects;
        }

        public override string Message
        {
            get
            {
                if (ParamObjects != null && ParamObjects.Length > 0)
                {
                    return string.Format(base.Message, ParamObjects);
                }
                return base.Message;
                
            }
        }
    }
}
