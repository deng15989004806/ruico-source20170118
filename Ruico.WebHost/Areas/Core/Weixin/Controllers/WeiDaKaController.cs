using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ruico.Application.Exceptions;
using Ruico.Application.KaoQinModule;
using Ruico.Dto.KaoQin;
using Ruico.Infrastructure.Utility.Helper;
using Ruico.WebHost.Models;
using Ruico.WebHost.Resources.Generated;

namespace Ruico.WebHost.Areas.Core.Weixin.Controllers
{
    public class WeiDaKaController : CoreBaseController
    {
        private IWeiDaKaService _weiDaKaService;

        public WeiDaKaController(IWeiDaKaService weiDaKaService)
        {
            _weiDaKaService = weiDaKaService;
        }

        // GET: Core/Weixin/WeiDaKa
        public ActionResult Index(DateTime? dateStart, DateTime? dateEnd, KaoQinStatusDTO? status, string userId,
            int? page)
        {
            var conditon = new KaoQinConditionDTO()
            {
                CreatedStartTime = DateTimeHelper.GetDateStart(dateStart),
                CreatedEndTime = DateTimeHelper.GetDateEnd(dateEnd),
                UserId = userId
            };
            conditon.Statuses = new List<string>();
            if (status.HasValue)
            {
                conditon.Statuses.Add(status.ToString());
            }
            var list = _weiDaKaService.FindBy(conditon, page.HasValue ? page.Value : 1,
                CustomDisplayExtensions.DefaultPageSize);

            ViewBag.Condition = conditon;

            return View(list);
        }

        [HttpPost]
        public ActionResult SearchWeiDaKa(DateTime? dateStart, DateTime? dateEnd, KaoQinStatusDTO? status, string userId,
            bool export)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                if (dateStart.HasValue && dateEnd.HasValue
                    && dateStart.Value > dateEnd.Value)
                {
                    throw new DataNotFoundException(MessagesResources.StartTime_Greater_Than_EndTime);
                }

                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action(export ? "Export" : "Index", new
                    {
                        dateStart,
                        dateEnd,
                        status,
                        userId,
                        t = DateTime.Now.Ticks
                    })
                });
            });
        }

        public ActionResult Export(DateTime? dateStart, DateTime? dateEnd, KaoQinStatusDTO? status, string userId)
        {
            var conditon = new KaoQinConditionDTO()
            {
                CreatedStartTime = DateTimeHelper.GetDateStart(dateStart),
                CreatedEndTime = DateTimeHelper.GetDateEnd(dateEnd),
                UserId = userId
            };
            conditon.Statuses = new List<string>();
            if (status.HasValue)
            {
                conditon.Statuses.Add(status.ToString());
            }
            var list = _weiDaKaService.FindBy(conditon, 1, int.MaxValue);

            var dt = _weiDaKaService.GetExportTable(list.ToList());
            var ms = ExcelHelper.DataTableToStream(dt, dt.TableName);
            var fileName = string.Format("{0}-{1}.xls", dt.TableName, DateTimeHelper.GetDateTimeRandomStr(DateTime.Now));
            return File(ms, HttpHelper.ResponseContentTypeExcel, fileName);
        }
    }
}