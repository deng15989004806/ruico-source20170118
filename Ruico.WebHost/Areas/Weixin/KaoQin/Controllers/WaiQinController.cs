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
    public class WaiQinController : KaoQinBaseController
    {
        private IWaiQinService _waiQinService;

        public WaiQinController(IMemberService memberService,
            IWaiQinService waiQinService) 
            : base(memberService)
        {
            _waiQinService = waiQinService;
        }

        [Route("Weixin/KaoQin/WaiQin")]
        public ActionResult Index(int? page)
        {
            var list = _waiQinService.FindBy(new KaoQinConditionDTO()
            {
                UserId = CurrentUserId,
                CreatedStartTime = GetConditionCreatedStartTime()
            }, page.HasValue ? page.Value : 1, 5);

            return View(list);
        }

        [Route("Weixin/KaoQin/WaiQin/Apply")]
        public ActionResult Apply()
        {
            return View();
        }

        [Route("Weixin/KaoQin/WaiQin/Apply")]
        [HttpPost]
        public ActionResult Apply(WaiQinDTO item)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                var department = GetCurrentMemberDepartment();
                item.UserId = CurrentMember.Userid;
                item.Name = CurrentMember.Name;
                item.Position = CurrentMember.Position;
                item.DepartmentId = department.DepartmentId;
                item.Department = department.Name;
                _waiQinService.Add(item, GetCurrentOperator());
                this.JsMessage = MessagesResources.Add_Success;

                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                });
            });
        }

        [Route("Weixin/KaoQin/WaiQin/Cancel")]
        [HttpPost]
        public ActionResult Cancel(Guid id)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                _waiQinService.Cancel(id, GetCurrentOperator());

                this.JsMessage = MessagesResources.Cancel_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                }, JsonRequestBehavior.AllowGet);
            });
        }

        [Route("Weixin/KaoQin/WaiQin/PendingApprove")]
        public ActionResult PendingApprove(int? page)
        {
            if (!IsWeiXinManager())
            {
                return NotWeiXinManager();
            }

            var condition = GetApproveKaoQinCondition(CurrentUserId, CurrentMember.Position,
                new List<string>() { KaoQinStatusDTO.Submited.ToString() });
            var list = _waiQinService.FindBy(condition, page.HasValue ? page.Value : 1, 5);

            return View(list);
        }

        [Route("Weixin/KaoQin/WaiQin/Approve")]
        public ActionResult Approve(Guid id)
        {
            if (!IsWeiXinManager())
            {
                return NotWeiXinManager();
            }

            var waiQinDTO = _waiQinService.FindBy(id) ?? new WaiQinDTO();

            return View(waiQinDTO);
        }

        [Route("Weixin/KaoQin/WaiQin/Approve")]
        [HttpPost]
        public ActionResult Approve(WaiQinDTO item)
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
                _waiQinService.Approve(item, GetCurrentOperator());

                this.JsMessage = MessagesResources.Approve_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Approved")
                }, JsonRequestBehavior.AllowGet);
            });
        }

        [Route("Weixin/KaoQin/WaiQin/Approved")]
        public ActionResult Approved(int? page)
        {
            if (!IsWeiXinManager())
            {
                return NotWeiXinManager();
            }

            var condition = GetApproveKaoQinCondition(CurrentUserId, CurrentMember.Position,
                new List<string>() { KaoQinStatusDTO.Approved.ToString() });
            var list = _waiQinService.FindBy(condition, page.HasValue ? page.Value : 1, 5);

            return View(list);
        }
    }
}