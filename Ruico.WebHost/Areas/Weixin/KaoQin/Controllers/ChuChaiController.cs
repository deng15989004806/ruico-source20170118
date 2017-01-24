using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ruico.Application.Exceptions;
using Ruico.Application.HrModule;
using Ruico.Application.KaoQinModule;
using Ruico.Domain.Weixin.Service;
using Ruico.Dto.KaoQin;
using Ruico.WebHost.Models;
using Ruico.WebHost.Resources.Generated;

namespace Ruico.WebHost.Areas.Weixin.KaoQin.Controllers
{
    public class ChuChaiController : KaoQinBaseController
    {
        private IChuChaiService _chuChaiService;

        public ChuChaiController(IMemberService memberService,
            IChuChaiService chuChaiService) 
            : base(memberService)
        {
            _chuChaiService = chuChaiService;
        }

        [Route("Weixin/KaoQin/ChuChai")]
        public ActionResult Index(int? page)
        {
            var list = _chuChaiService.FindBy(new KaoQinConditionDTO()
            {
                UserId = CurrentUserId,
                CreatedStartTime = GetConditionCreatedStartTime()
            }, page.HasValue ? page.Value : 1, 5);

            return View(list);
        }

        [Route("Weixin/KaoQin/ChuChai/Apply")]
        public ActionResult Apply()
        {
            return View();
        }

        [Route("Weixin/KaoQin/ChuChai/Apply")]
        [HttpPost]
        public ActionResult Apply(ChuChaiDTO item)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                var department = GetCurrentMemberDepartment();
                item.UserId = CurrentMember.Userid;
                item.Name = CurrentMember.Name;
                item.Position = CurrentMember.Position;
                item.DepartmentId = department.DepartmentId;
                item.Department = department.Name;
                _chuChaiService.Add(item, GetCurrentOperator());
                this.JsMessage = MessagesResources.Add_Success;

                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                });
            });
        }

        [Route("Weixin/KaoQin/ChuChai/Cancel")]
        [HttpPost]
        public ActionResult Cancel(Guid id)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                _chuChaiService.Cancel(id, GetCurrentOperator());

                this.JsMessage = MessagesResources.Cancel_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                }, JsonRequestBehavior.AllowGet);
            });
        }

        [Route("Weixin/KaoQin/ChuChai/PendingApprove")]
        public ActionResult PendingApprove(int? page)
        {
            if (!IsWeiXinManager())
            {
                return NotWeiXinManager();
            }

            var condition = GetApproveKaoQinCondition(CurrentUserId, CurrentMember.Position,
                new List<string>() {KaoQinStatusDTO.Submited.ToString()});
            var list = _chuChaiService.FindBy(condition, page.HasValue ? page.Value : 1, 5);

            return View(list);
        }

        [Route("Weixin/KaoQin/ChuChai/Approve")]
        public ActionResult Approve(Guid id)
        {
            if (!IsWeiXinManager())
            {
                return NotWeiXinManager();
            }

            var chuChaiDTO = _chuChaiService.FindBy(id) ?? new ChuChaiDTO();

            return View(chuChaiDTO);
        }

        [Route("Weixin/KaoQin/ChuChai/Approve")]
        [HttpPost]
        public ActionResult Approve(ChuChaiDTO item)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                if (!IsWeiXinManager())
                {
                    throw new DefinedException(NotWeiXinManagerMessage);
                }

                item.DepartmentOpinionApproverId = CurrentMember.Userid;
                item.GeneralManagerOfficeOpinionApproverId = CurrentMember.Userid;
                item.CompanyLeaderOpinionApproverId = CurrentMember.Userid;
                _chuChaiService.Approve(item, GetCurrentOperator());

                this.JsMessage = MessagesResources.Approve_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Approved")
                }, JsonRequestBehavior.AllowGet);
            });
        }

        [Route("Weixin/KaoQin/ChuChai/Approved")]
        public ActionResult Approved(int? page)
        {
            if (!IsWeiXinManager())
            {
                return NotWeiXinManager();
            }

            var condition = GetApproveKaoQinCondition(CurrentUserId, CurrentMember.Position,
                new List<string>() { KaoQinStatusDTO.Approved.ToString() });
            var list = _chuChaiService.FindBy(condition, page.HasValue ? page.Value : 1, 5);

            return View(list);
        }
    }
}