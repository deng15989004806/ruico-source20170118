using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ruico.Application.SystemModule;
using Ruico.Dto.System;
using Ruico.Infrastructure.Utility.Helper;
using Ruico.WebHost.Models;

namespace Ruico.WebHost.Areas.Core.System.Controllers
{
    public class OperateLogController : CoreBaseController
    {
        IOperateRecordService _operateRecordService;

        public OperateLogController(IOperateRecordService operateRecordService)
        {
            _operateRecordService = operateRecordService;
        }

        // GET: System/OperateLog
        public ActionResult Index(string sn)
        {
            ViewBag.Sn = sn;

            var list = _operateRecordService.FindBy(sn, 1, 50);
            return View(list.ToList());
        }

        [HttpPost]
        public ActionResult SearchLog(string sn)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index", new
                    {
                        sn,
                    })
                });
            });
        }

        // GET: System/OperateLog/List
        public ActionResult List(string sn)
        {
            ViewBag.Sn = sn;

            var list = new List<OperateRecordDTO>();
            if (sn.NotNullOrBlank())
            {
                list = _operateRecordService.FindBy(sn, 1, 50).ToList();
            }
            return View(list);
        }
    }
}