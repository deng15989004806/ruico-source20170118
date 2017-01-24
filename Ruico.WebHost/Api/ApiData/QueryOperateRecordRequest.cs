namespace Ruico.WebHost.Api.ApiData
{
    public class QueryOperateRecordRequest : ApiRequestBase
    {
        public string Sn { get; set; }

        /// <summary>
        /// 最多取几个记录（默认50个，最多200个）
        /// </summary>
        public string LimitCount { get; set; }
    }
}