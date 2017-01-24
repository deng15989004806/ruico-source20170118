using System;
using System.Web.Mvc;
using log4net;

namespace Ruico.Application.SystemModule.Imp
{
    public class SerialNumberGenerator
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SerialNumberGenerator));

        protected static IServiceResolver ServiceResolver
        {
            get { return (IServiceResolver)DependencyResolver.Current.GetService(typeof(IServiceResolver)); }
        }

        protected static ISerialNumberService SerialNumberService
        {
            get { return ServiceResolver.Resolve<ISerialNumberService>(); }
        }

        /// <summary>
        /// 返回序列号序号
        /// </summary>
        /// <param name="prefix">序列号前缀</param>
        /// <param name="dateNumber">日期编号</param>
        /// <param name="increase">流水号增量</param>
        /// <returns></returns>
        public static int GetSerialNumber(string prefix, string dateNumber, int increase)
        {
            return SerialNumberService.GetSerialNumber(prefix, dateNumber, increase);
        }

        /// <summary>
        /// 返回序列号编号
        /// </summary>
        /// <param name="prefix">序列号前缀</param>
        /// <param name="useDateNumber">是否使用日期编号</param>
        /// <param name="numberLength">流水号长度</param>
        /// <returns></returns>
        public static string GetSerialNumber(string prefix, bool useDateNumber, int numberLength)
        {
            var dateNumber = useDateNumber ? DateTime.UtcNow.ToString("yyyyMMdd") : string.Empty;
            const int increase = 1;
            var number = GetSerialNumber(prefix, dateNumber, increase);

            var max = Math.Pow(10, numberLength);
            if (number >= max)
            {
                throw new Exception("serial number can not be greater than " + (max - 1));
            }

            return string.Format("{0}{1}{2}", prefix, dateNumber, number.ToString("0")
                .PadLeft(numberLength, '0'));
        }
    }
}
