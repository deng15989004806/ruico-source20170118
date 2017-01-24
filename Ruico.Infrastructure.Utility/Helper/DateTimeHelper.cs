using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruico.Infrastructure.Utility.Helper
{
    public class DateTimeHelper
    {
        public static DateTime GetDateStart(DateTime dateTime)
        {
            return dateTime.Date;
        }

        public static DateTime GetDateEnd(DateTime dateTime)
        {
            return dateTime.Date.AddDays(1);
        }

        public static DateTime? GetDateStart(DateTime? dateTime)
        {
            return dateTime.HasValue ? GetDateStart(dateTime.Value) : (DateTime?)null;
        }

        public static DateTime? GetDateEnd(DateTime? dateTime)
        {
            return dateTime.HasValue ? GetDateEnd(dateTime.Value) : (DateTime?)null;
        }

        public static string GetDateTimeRandomStr(DateTime? dt)
        {
            if (!dt.HasValue)
            {
                dt = DateTime.Now;
            }

            var rnd = new Random(SecurityHelper.GetRandomSeed());
            return dt.Value.ToString("yyyyMMddHHmmss") + rnd.NetxtString("0123456789", 4);
        }
    }
}
