namespace Ruico.WebHost.Api.ApiData
{
    public class AddOperateRecordRequest : ApiRequestBase
    {
        public string Sn { get; set; }

        public string UserId { get; set; }

        public string OperatorName { get; set; }

        public string Operation { get; set; }

        public string Message { get; set; }

        public string OperateTime { get; set; }

        public string Ip { get; set; }
    }
}