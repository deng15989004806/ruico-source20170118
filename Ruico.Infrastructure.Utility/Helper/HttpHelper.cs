using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Ruico.Infrastructure.Utility.Helper
{
    public class HttpHelper
    {
        public static readonly string ResponseContentTypeExcel = "application/ms-excel";

        public static string GetRealIP()
        {
            string result = String.Empty;

            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            //可能有代理 
            if (!string.IsNullOrWhiteSpace(result))
            {
                //没有"." 肯定是非IP格式
                if (result.IndexOf(".") == -1)
                {
                    result = null;
                }
                else
                {
                    //有","，估计多个代理。取第一个不是内网的IP。
                    if (result.IndexOf(",") != -1)
                    {
                        result = result.Replace(" ", string.Empty).Replace("\"", string.Empty);

                        var temparyIp = result.Split(",;".ToCharArray());

                        if (temparyIp != null && temparyIp.Length > 0)
                        {
                            for (int i = 0; i < temparyIp.Length; i++)
                            {
                                //找到不是内网的地址
                                if (IsIPAddress(temparyIp[i])
                                    && temparyIp[i].Substring(0, 3) != "10."
                                    && temparyIp[i].Substring(0, 7) != "192.168"
                                    && temparyIp[i].Substring(0, 7) != "172.16.")
                                {
                                    return temparyIp[i];
                                }
                            }
                        }
                    }
                        //代理即是IP格式
                    else if (IsIPAddress(result))
                    {
                        return result;
                    }
                        //代理中的内容非IP
                    else
                    {
                        result = null;
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (string.IsNullOrWhiteSpace(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }

            return result;
        }

        public static bool IsIPAddress(string str)
        {
            if (string.IsNullOrWhiteSpace(str) || str.Length < 7 || str.Length > 15)
                return false;

            const string regFormat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}{1}";

            var regex = new Regex(regFormat, RegexOptions.IgnoreCase);

            return regex.IsMatch(str);
        }

        public static String UrlEncodeU8(String s)
        {
            return HttpUtility.UrlEncode(s, Encoding.UTF8);
        }

        /// <summary>
        /// 通过GET方式获取页面的方法
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <returns></returns>
        public static string HttpGetRequest(string url)
        {
            //return HttpGet(url, Encoding.Default);
            return HttpGetRequest(url, Encoding.UTF8);
        }
        /// <summary>
        /// 通过GET方式获取页面的方法
        /// </summary>
        /// <param name="uri">请求的URL</param>
        /// <param name="encoding">页面编码</param>
        /// <returns></returns>
        public static string HttpGetRequest(string uri, Encoding encoding)
        {
            //定义局部变量
            HttpWebRequest httpWebRequest;
            HttpWebResponse httpWebRespones;
            Stream stream;
            string htmlString;

            #region 请求页面
            try
            {
                httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;
            }
            //处理异常
            catch (WebException ex)
            {
                throw ex;
            }
            #endregion

            #region 获取服务器的返回信息
            try
            {
                httpWebRespones = (HttpWebResponse)httpWebRequest.GetResponse();
                stream = httpWebRespones.GetResponseStream();
            }
            //处理异常
            catch (WebException ex)
            {
                //var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                throw ex;
            }

            var streamReader = new StreamReader(stream, encoding);
            if (httpWebRespones.Headers["Content-Encoding"].NotNullOrBlank() && httpWebRespones.Headers["Content-Encoding"].Equals("gzip"))
            {
                var gzipStream = new GZipStream(stream, CompressionMode.Decompress);
                streamReader = new StreamReader(gzipStream);
            }
            #endregion

            #region 读取返回页面
            try
            {
                htmlString = streamReader.ReadToEnd();
            }
            //处理异常
            catch (WebException ex)
            {
                //var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                throw ex;
            }
            //释放资源返回结果
            finally
            {
                streamReader.Close();
                stream.Close();
            }
            #endregion

            return htmlString;
        }

        public static String HttpPostRequest(string uri, Dictionary<String, String> paramters, Dictionary<String, String> headers = null)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            var paramStr = new StringBuilder("");

            foreach (var pair in paramters)
            {
                if (pair.Value != null)
                {
                    if (pair.Key.Equals("key", StringComparison.OrdinalIgnoreCase))
                    {
                        paramStr.AppendFormat("{0}={1}&", pair.Key.Trim(), pair.Value.Trim());
                    }
                    else
                    {
                        paramStr.AppendFormat("{0}={1}&", UrlEncodeU8(pair.Key.Trim()), UrlEncodeU8(pair.Value.Trim()));
                    }
                }
            }

            if (headers != null)
            {
                foreach (var pair in headers)
                {
                    webRequest.Headers.Add(pair.Key, pair.Value);
                }
            }

            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";

            byte[] bytes = Encoding.ASCII.GetBytes(paramStr.ToString().TrimEnd('&'));
            Stream os = null;
            try
            {
                webRequest.ContentLength = bytes.Length;
                os = webRequest.GetRequestStream();
                os.Write(bytes, 0, bytes.Length);         //Send it
            }
            //处理异常
            catch (WebException ex)
            {
                //var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                throw ex;
            }
            finally
            {
                if (os != null)
                {
                    os.Close();
                }
            }

            try
            {
                HttpWebResponse webResponse;
                string res = "";
                using (webResponse = (HttpWebResponse)webRequest.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                    {
                        res = reader.ReadToEnd();
                    }
                }
                return res;
            }
            //处理异常
            catch (WebException ex)
            {
                //var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                throw ex;
            }
        }
    }
}
