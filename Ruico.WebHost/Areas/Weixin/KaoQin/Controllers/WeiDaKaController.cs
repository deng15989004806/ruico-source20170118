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
    public class WeiDaKaController : KaoQinBaseController
    {
        private IWeiDaKaService _weiDaKaService;

        public WeiDaKaController(IMemberService memberService,
            IWeiDaKaService weiDaKaService) 
            : base(memberService)
        {
            _weiDaKaService = weiDaKaService;
        }

        [Route("Weixin/KaoQin/WeiDaKa")]
        public ActionResult Index(int? page)
        {
            var list = _weiDaKaService.FindBy(new KaoQinConditionDTO()
            {
                UserId = CurrentUserId,
                CreatedStartTime = GetConditionCreatedStartTime()
            }, page.HasValue ? page.Value : 1, 5);

            return View(list);
        }

        [Route("Weixin/KaoQin/WeiDaKa/Apply")]
        public ActionResult Apply()
        {
            return View();
        }

        [Route("Weixin/KaoQin/WeiDaKa/Apply")]
        [HttpPost]
        public ActionResult Apply(WeiDaKaDTO item)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                var department = GetCurrentMemberDepartment();
                item.UserId = CurrentMember.Userid;
                item.Name = CurrentMember.Name;
                item.Position = CurrentMember.Position;
                item.DepartmentId = department.DepartmentId;
                item.Department = department.Name;
                _weiDaKaService.Add(item, GetCurrentOperator());
                this.JsMessage = MessagesResources.Add_Success;

                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                });
            });
        }

        [Route("Weixin/KaoQin/WeiDaKa/Cancel")]
        [HttpPost]
        public ActionResult Cancel(Guid id)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                _weiDaKaService.Cancel(id, GetCurrentOperator());

                this.JsMessage = MessagesResources.Cancel_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                }, JsonRequestBehavior.AllowGet);
            });
        }

        [Route("Weixin/KaoQin/WeiDaKa/PendingApprove")]
        public ActionResult PendingApprove(int? page)
        {
            if (!IsWeiXinManager())
            {
                return NotWeiXinManager();
            }

            var condition = GetApproveKaoQinCondition(CurrentUserId, CurrentMember.Position,
                new List<string>() { KaoQinStatusDTO.Submited.ToString() });
            var list = _weiDaKaService.FindBy(condition, page.HasValue ? page.Value : 1, 5);

            return View(list);
        }

        [Route("Weixin/KaoQin/WeiDaKa/Approve")]
        public ActionResult Approve(Guid id)
        {
            if (!IsWeiXinManager())
            {
                return NotWeiXinManager();
            }

            var weiDaKaDTO = _weiDaKaService.FindBy(id) ?? new WeiDaKaDTO();

            return View(weiDaKaDTO);
        }

        [Route("Weixin/KaoQin/WeiDaKa/Approve")]
        [HttpPost]
        public ActionResult Approve(WeiDaKaDTO item)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                if (!IsWeiXinManager())
                {
                    throw new DefinedException(NotWeiXinManagerMessage);
                }

                item.DepartmentOrCompanyOpinionApproverId = CurrentMember.Userid;
                _weiDaKaService.Approve(item, GetCurrentOperator());

                this.JsMessage = MessagesResources.Approve_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Approved")
                }, JsonRequestBehavior.AllowGet);
            });
        }

        [Route("Weixin/KaoQin/WeiDaKa/Approved")]
        public ActionResult Approved(int? page)
        {
            if (!IsWeiXinManager())
            {
                return NotWeiXinManager();
            }

            var condition = GetApproveKaoQinCondition(CurrentUserId, CurrentMember.Position,
                new List<string>() { KaoQinStatusDTO.Approved.ToString() });
            var list = _weiDaKaService.FindBy(condition, page.HasValue ? page.Value : 1, 5);

            return View(list);
        }
    }
}