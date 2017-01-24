using System;

namespace Ruico.Infrastructure.Authorize
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class PermissionAttribute : Attribute
    {
        public PermissionAttribute(string code)
        {
            Code = code;
        }

        public PermissionAttribute(string code, int sortOrder)
        {
            Code = code;
            SortOrder = sortOrder;
        }

        public string Code { get; private set; }

        public int SortOrder { get; private set; }
    }
}
