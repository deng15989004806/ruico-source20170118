using System;
using System.Configuration;
using System.Text;

namespace Ruico.Infrastructure.Utility.Helper
{
    public static class SecurityHelper
    {
        private static string AuthSecret
        {
            get
            {
                var authSecret = ConfigurationManager.AppSettings["Ruico:AuthSecret"];
                if (authSecret.IsNullOrBlank())
                {
                    throw new ConfigurationErrorsException("missing \"Ruico:AuthSecret\" appSetting.");
                }
                return authSecret;
            }
        }

        public static int GetRandomSeed()
        {
            var bytes = new byte[4];
            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary> 
        /// 0~9 A~Z字符串 
        /// </summary> 
        public static string RandomString09Az = "0123456789ABCDEFGHIJKMLNOPQRSTUVWXYZ";

        public static string NetxtString(int size)
        {
            var rnd = new Random(GetRandomSeed());
            return rnd.NetxtString(RandomString09Az, size);
        }

        /// <summary> 
        /// 依据指定字符串来生成随机字符串 
        /// </summary> 
        /// <param name="random">Random</param> 
        /// <param name="randomString">指定字符串</param> 
        /// <param name="size">字符串长度</param> 
        /// <returns>随机字符串</returns> 
        public static string NetxtString(this Random random, string randomString, int size)
        {
            var nextString = string.Empty;
            if (random == null || string.IsNullOrEmpty(randomString)) return nextString;
            var builder = new StringBuilder(size);
            int maxCount = randomString.Length - 1;
            for (var i = 0; i < size; i++)
            {
                var number = random.Next(0, maxCount);
                builder.Append(randomString[number]);
            }
            nextString = builder.ToString();
            return nextString;
        }

        public static string EncryptPassword(string password)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password + "_" + AuthSecret);
            byte[] sha1Bytes = System.Security.Cryptography.SHA1.Create().ComputeHash(bytes);
            string secretPart = BitConverter.ToString(sha1Bytes).Replace("-", string.Empty);
            return secretPart;
        }

        public static string Encrypt(string str, string secret)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str + "_" + secret);
            byte[] sha1Bytes = System.Security.Cryptography.SHA1.Create().ComputeHash(bytes);
            string secretPart = BitConverter.ToString(sha1Bytes).Replace("-", string.Empty);
            return secretPart;
        }

        /// <summary>
        /// 获取相对1970年1月1日凌晨的时间戳(请传入UTC时间)
        /// </summary>
        /// <param name="utcDateTime"></param>
        /// <returns></returns>
        public static long GetTimestampAsPhp(DateTime utcDateTime)
        {
            // 相对1970年1月1日凌晨的时间戳
            var timeStamp = new DateTime(1970, 1, 1);
            // 注意这里有时区问题，用now就要减掉8个小时
            long stamp = (utcDateTime.Ticks - timeStamp.Ticks) / 10000000;

            return stamp;
        }

        /// <summary>
        /// 时间戳转换为UTC时间
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        public static DateTime GetTimeFromTimestamp(long timestamp)
        {
            // 相对1970年1月1日凌晨的时间戳
            var time = new DateTime(1970, 1, 1);

            return time.AddSeconds(timestamp);
        }
    }
}
