using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Ruico.Application.SystemModule;
using Ruico.Dto.Common;
using Ruico.Dto.System;
using Ruico.Infrastructure.Utility.Helper;
using Ruico.WebHost.Api.ApiData;

namespace Ruico.WebHost.Api
{
    public class OperateRecordController : ApiControllerBase
    {
        IOperateRecordService _service;

        public OperateRecordController(IOperateRecordService service)
        {
            _service = service;
        }

        [System.Web.Mvc.Route("/Api/OperateRecord/Save")]
        [System.Web.Mvc.HttpPost]
        public HttpResponseMessage Save([FromBody]AddOperateRecordRequest request)
        {
            var response = new WebApiResponseDTO<string>();
            try
            {
                VerifyAuth(request);

                if (request.Sn.IsNullOrBlank())
                {
                    throw new Exception("Sn is empty");
                }

                if (request.OperateTime.IsNullOrBlank())
                {
                    throw new Exception("OperateTime is empty");
                }

                var result = _service.Add(new OperateRecordDTO()
                {
                    Sn = request.Sn,
                    Operation = request.Operation,
                    Message = request.Message,
                    OperatorName = request.OperatorName,
                    OperateTime = DateTime.Parse(request.OperateTime),
                    UserId = request.UserId.IsNullOrBlank() ? (Guid?) null : Guid.Parse(request.UserId),
                    Ip = request.Ip,
                });

                response.Succeeded = true;
                response.Message = "ok";
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                response.Message = ex.Message;
            }

            return GetJsonHttpResponseMessage(response);
        }

        [System.Web.Mvc.Route("/Api/OperateRecord/Query")]
        [System.Web.Mvc.HttpPost]
        public HttpResponseMessage Query([FromBody]QueryOperateRecordRequest request)
        {
            var response = new WebApiResponseDTO<List<OperateRecordDTO>>();
            try
            {
                VerifyAuth(request);

                var limitCount = 50;

                if (request.LimitCount.NotNullOrBlank() && request.LimitCount != "0")
                {
                    if (!ValidatePositiveInteger(request.LimitCount))
                    {
                        throw new Exception("LimitCount invalid: " + request.Timestamp);
                    }

                    limitCount = Convert.ToInt32(request.LimitCount);
                    if (limitCount < 0 || limitCount > 200)
                    {
                        throw new Exception("LimitCount must between 1~200");
                    }
                }

                var result = _service.FindBy(request.Sn, 1, limitCount);

                response.Succeeded = true;
                response.Message = "ok";
                response.Data = result.ToList();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                response.Message = ex.Message;
            }

            return GetJsonHttpResponseMessage(response);
        }
    }
}
