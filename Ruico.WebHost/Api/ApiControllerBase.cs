using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using log4net;
using Newtonsoft.Json;
using Ruico.Infrastructure.Utility.Helper;

namespace Ruico.WebHost.Api
{
    public class ApiRequestBase
    {
        /// <summary>
        /// 时间戳
        /// </summary>
        public string Timestamp { get; set; }

        /// <summary>
        /// 签名字符串，使用Secret和签名算法进行签名
        /// </summary>
        public string Signature { get; set; }
    }

    public class ApiControllerBase : System.Web.Http.ApiController
    {
        protected static readonly ILog Logger = LogManager.GetLogger(typeof(ApiControllerBase));

        private static readonly Regex  PositiveIntegerRe = new Regex(@"^[1-9]\d*$", RegexOptions.Compiled); 

        protected static string WebApiSecret
        {
            get
            {
                var url = ConfigurationManager.AppSettings["Ruico:WebApiSecret"];
                if (url.IsNullOrBlank())
                {
                    throw new ConfigurationErrorsException("missing \"Ruico:WebApiSecret\" appSetting.");
                }
                return url;
            }
        }

        protected bool ValidatePositiveInteger(string input)
        {
            return PositiveIntegerRe.IsMatch(input);
        }

        /// <summary>
        /// 验证授权
        /// </summary>
        protected virtual void VerifyAuth(ApiRequestBase request)
        {
            if (request == null)
            {
                throw new Exception("Request is null");
            }

            if (request.Timestamp.IsNullOrBlank())
            {
                throw new Exception("Timestamp is empty");
            }

            if (!ValidatePositiveInteger(request.Timestamp))
            {
                throw new Exception("Timestamp invalid: " + request.Timestamp);
            }

            if (request.Signature.IsNullOrBlank())
            {
                throw new Exception("Signature is empty");
            }

            var dateTime = SecurityHelper.GetTimeFromTimestamp(Convert.ToInt64(request.Timestamp));
            if (TimeSpan.FromTicks(Math.Abs(dateTime.Ticks - DateTime.UtcNow.Ticks)).Minutes > 15)
            {
                throw new Exception("Current time difference of more than 15 minutes with the server. timestamp: " + request.Timestamp);
            }

            // 验证签名
            if (!SecurityHelper.Encrypt(request.Timestamp, WebApiSecret).EqualsIgnoreCase(request.Signature))
            {
                throw new Exception("Signature invalid");
            }
        }

        protected HttpResponseMessage GetJsonHttpResponseMessage<T>(T obj)
        {
            var responseJson = JsonConvert.SerializeObject(obj);
            var resp = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            };
            return resp;
        }
    }
}