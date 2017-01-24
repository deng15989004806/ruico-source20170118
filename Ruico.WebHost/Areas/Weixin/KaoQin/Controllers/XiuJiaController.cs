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
    public class XiuJiaController : KaoQinBaseController
    {
        private IXiuJiaService _xiuJiaService;

        public XiuJiaController(IMemberService memberService,
            IXiuJiaService xiuJiaService) 
            : base(memberService)
        {
            _xiuJiaService = xiuJiaService;
        }

        [Route("Weixin/KaoQin/XiuJia")]
        public ActionResult Index(int? page)
        {
            var list = _xiuJiaService.FindBy(new KaoQinConditionDTO()
            {
                UserId = CurrentUserId,
                CreatedStartTime = GetConditionCreatedStartTime()
            }, page.HasValue ? page.Value : 1, 5);

            return View(list);
        }

        [Route("Weixin/KaoQin/XiuJia/Apply")]
        public ActionResult Apply()
        {
            return View();
        }

        [Route("Weixin/KaoQin/XiuJia/Apply")]
        [HttpPost]
        public ActionResult Apply(XiuJiaDTO item)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                var department = GetCurrentMemberDepartment();
                item.UserId = CurrentMember.Userid;
                item.Name = CurrentMember.Name;
                item.Position = CurrentMember.Position;
                item.DepartmentId = department.DepartmentId;
                item.Department = department.Name;
                _xiuJiaService.Add(item, GetCurrentOperator());
                this.JsMessage = MessagesResources.Add_Success;

                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                });
            });
        }

        [Route("Weixin/KaoQin/XiuJia/Cancel")]
        [HttpPost]
        public ActionResult Cancel(Guid id)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                _xiuJiaService.Cancel(id, GetCurrentOperator());

                this.JsMessage = MessagesResources.Cancel_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                }, JsonRequestBehavior.AllowGet);
            });
        }

        [Route("Weixin/KaoQin/XiuJia/PendingApprove")]
        public ActionResult PendingApprove(int? page)
        {
            if (!IsWeiXinManager())
            {
                return NotWeiXinManager();
            }

            var condition = GetApproveKaoQinCondition(CurrentUserId, CurrentMember.Position,
                new List<string>() { KaoQinStatusDTO.Submited.ToString() });
            var list = _xiuJiaService.FindBy(condition, page.HasValue ? page.Value : 1, 5);

            return View(list);
        }

        [Route("Weixin/KaoQin/XiuJia/Approve")]
        public ActionResult Approve(Guid id)
        {
            if (!IsWeiXinManager())
            {
                return NotWeiXinManager();
            }

            var xiuJiaDTO = _xiuJiaService.FindBy(id) ?? new XiuJiaDTO();

            return View(xiuJiaDTO);
        }

        [Route("Weixin/KaoQin/XiuJia/Approve")]
        [HttpPost]
        public ActionResult Approve(XiuJiaDTO item)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                if (!IsWeiXinManager())
                {
                    throw new DefinedException(NotWeiXinManagerMessage);
                }

                item.DepartmentSupervisorOpinionApproverId = CurrentMember.Userid;
                item.DepartmentManagerOpinionApproverId = CurrentMember.Userid;
                item.CompanyLeaderOpinionApproverId = CurrentMember.Userid;
                _xiuJiaService.Approve(item, GetCurrentOperator());

                this.JsMessage = MessagesResources.Approve_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Approved")
                }, JsonRequestBehavior.AllowGet);
            });
        }

        [Route("Weixin/KaoQin/XiuJia/Approved")]
        public ActionResult Approved(int? page)
        {
            if (!IsWeiXinManager())
            {
                return NotWeiXinManager();
            }

            var condition = GetApproveKaoQinCondition(CurrentUserId, CurrentMember.Position,
                new List<string>() { KaoQinStatusDTO.Approved.ToString() });
            var list = _xiuJiaService.FindBy(condition, page.HasValue ? page.Value : 1, 5);

            return View(list);
        }
    }
}