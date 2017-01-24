using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Newtonsoft.Json;
using Ruico.Dto.Common;
using Ruico.Dto.System;
using Ruico.Infrastructure.Utility.Helper;

namespace Ruico.Application.SystemModule.Imp
{
    public partial class OperateRecorder
    {
        private static readonly ConcurrentQueue<OperateRecordDTO> LogQueue = 
            new ConcurrentQueue<OperateRecordDTO>();

        private static readonly Object Lock = new Object();

        private static readonly Object State = new Object();

        private static System.Threading.Timer _eventTimer;

        private const int SendIntervalInSecond = 1;

        protected static string OperateRecordSaveUrl
        {
            get
            {
                var url = ConfigurationManager.AppSettings["Ruico:OperateRecordSaveUrl"];
                if (url.IsNullOrBlank())
                {
                    throw new ConfigurationErrorsException("missing \"Ruico:OperateRecordSaveUrl\" appSetting.");
                }
                return url;
            }
        }
        protected static string OperateRecordQueryUrl
        {
            get
            {
                var url = ConfigurationManager.AppSettings["Ruico:OperateRecordQueryUrl"];
                if (url.IsNullOrBlank())
                {
                    throw new ConfigurationErrorsException("missing \"Ruico:OperateRecordQueryUrl\" appSetting.");
                }
                return url;
            }
        }

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

        /// <summary>
        /// 异步写操作日志
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="operation"></param>
        /// <param name="message"></param>
        public static void RecordOperationAsync(string sn, string operation, string message)
        {
            var ip = HttpHelper.GetRealIP();
            var @operator = GetCurrentOperator();
            var record = new OperateRecordDTO()
            {
                Sn = sn,
                UserId = @operator.UserId == Guid.Empty ? (Guid?)null : @operator.UserId,
                OperatorName = @operator.OperatorName,
                Operation = operation,
                OperateTime = DateTime.UtcNow,
                Message = message,
                Ip = ip
            };

            if (_eventTimer == null)
            {
                lock (Lock)
                {
                    _eventTimer = new System.Threading.Timer(
                        new System.Threading.TimerCallback(SendLogRecords),
                        State,
                        new TimeSpan(0, 0, 0),
                        new TimeSpan(0, 0, SendIntervalInSecond));
                }
            }

            LogQueue.Enqueue(record);
        }

        protected static Dictionary<String, String> ToPostDictionary(OperateRecordDTO record)
        {
            var timestamp = SecurityHelper.GetTimestampAsPhp(DateTime.UtcNow);
            return new Dictionary<String, String>
            {
                {"timestamp", Convert.ToString(timestamp)},
                {"signature", SecurityHelper.Encrypt(Convert.ToString(timestamp), WebApiSecret)},
                {"Sn", record.Sn},
                {"UserId", record.UserId.HasValue ? record.UserId.ToString() : string.Empty},
                {"OperatorName", record.OperatorName},
                {"Operation", record.Operation},
                {"Message", record.Message},
                {"OperateTime", record.OperateTime.ToString("yyyy-MM-dd HH:mm:ss.fff")},
                {"Ip", record.Ip}
            };
        }

        /// <summary>
        /// 查询操作记录
        /// </summary>
        /// <param name="sn">单据编号，为空时返回所有</param>
        /// <param name="searchArchive">是否搜索归档数据</param>
        /// <param name="limitCount">1~200，默认50</param>
        /// <param name="throwException">如果发生错误，是否抛出错误</param>
        /// <returns></returns>
        public static List<OperateRecordDTO> QueryOperateRecord(string sn, 
            int limitCount = 50, bool throwException = false)
        {
            try
            {
                var timestamp = SecurityHelper.GetTimestampAsPhp(DateTime.UtcNow);
                var parameters = new Dictionary<String, String>
                {
                    {"timestamp", Convert.ToString(timestamp)},
                    {"signature", SecurityHelper.Encrypt(Convert.ToString(timestamp), WebApiSecret)},
                    {"Sn", sn.IsNullOrBlank() ? string.Empty : sn},
                    {"LimitCount", Convert.ToString(limitCount)},
                };

                var json = HttpHelper.HttpPostRequest(OperateRecordQueryUrl, parameters);

                if (!json.StartsWith("{"))
                {
                    throw new Exception("invalid json string");
                }

                var result = JsonConvert.DeserializeObject<WebApiResponseDTO<List<OperateRecordDTO>>>(json);

                if (!result.Succeeded)
                {
                    throw new Exception("not succeeded, json string: " + json);
                }

                return result.Data;
            }
            catch (Exception ex)
            {
                var timestamp = SecurityHelper.GetTimestampAsPhp(DateTime.UtcNow);
                var parameters = new Dictionary<String, String>
                {
                    {"timestamp", Convert.ToString(timestamp)},
                    {"signature", SecurityHelper.Encrypt(Convert.ToString(timestamp), WebApiSecret)},
                    {"Sn", sn.IsNullOrBlank() ? string.Empty : sn},
                    {"LimitCount", Convert.ToString(limitCount)},
                };
                var sb = new StringBuilder("");
                sb.AppendFormat("{0}:{1}, ", "url", OperateRecordQueryUrl);
                foreach (var key in parameters.Keys)
                {
                    sb.AppendFormat("{0}:{1}, ", key, parameters[key]);
                }

                Logger.Error(String.Format("QueryOperateRecord Failed {0}", sb.ToString()), ex);

                if (throwException)
                {
                    throw;
                }
            }

            return new List<OperateRecordDTO>();
        }

        private static void SaveOperateRecordAsync(OperateRecordDTO record)
        {
            try
            {
                var parameters = ToPostDictionary(record);

                var json = HttpHelper.HttpPostRequest(OperateRecordSaveUrl, parameters);

                if (!json.StartsWith("{"))
                {
                    throw new Exception("invalid json string");
                }

                var result = JsonConvert.DeserializeObject<WebApiResponseDTO<string>>(json);

                if (!result.Succeeded)
                {
                    throw new Exception("not succeeded, json string: " + json);
                }
            }
            catch (Exception ex)
            {
                var parameters = ToPostDictionary(record);
                var sb = new StringBuilder("");
                sb.AppendFormat("{0}:{1}, ", "url", OperateRecordSaveUrl);
                foreach (var key in parameters.Keys)
                {
                    sb.AppendFormat("{0}:{1}, ", key, parameters[key]);
                }

                Logger.Error(String.Format("SaveOperateRecord Failed {0}", sb.ToString()), ex);
            }
        }

        private static void SendLogRecords(object sender)
        {
            try
            {
                OperateRecordDTO current = default(OperateRecordDTO);
                while (LogQueue.TryDequeue(out current))
                {
                    if (current == null)
                    {
                        break;
                    }
                    try
                    {
                        SaveOperateRecordAsync(current);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("", ex);
                        //LogQueue.Enqueue(current);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);
            }
        }
    }
}