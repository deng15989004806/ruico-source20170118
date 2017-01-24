using System;

namespace Ruico.Infrastructure.Authorize
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class MenuAttribute : Attribute
    {
        public MenuAttribute(string code)
        {
            Code = code;
            SortOrder = 99;
            Depth = code.Split('_').Length;
        }

        public MenuAttribute(string code, int sortOrder)
        {
            Code = code;
            SortOrder = sortOrder;
            Depth = code.Split('_').Length;
        }

        public string Code { get; private set; }

        public int SortOrder { get; private set; }

        public int Depth { get; private set; }
    }
}
