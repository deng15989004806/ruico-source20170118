using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Ruico.Application.HrModule;
using Ruico.Domain.Model;
using Ruico.Dto.Hr;
using Ruico.Dto.UserSystem;
using Ruico.Infrastructure.Utility.Helper;
using Ruico.WebHost.Controllers;

namespace Ruico.WebHost.Areas.Weixin
{
    public abstract class WeixinBaseController : BaseController
    {
        /// <summary>
        /// 微信审批权限，看职位
        /// </summary>
        /// <returns></returns>
        protected bool IsWeiXinManager()
        {
            return CurrentMember != null
                   && (CurrentMember.Position == MemberPositions.DepartmentSupervisor
                       || CurrentMember.Position == MemberPositions.DepartmentManager
                       || CurrentMember.Position == MemberPositions.CompanyLeader);
        }

        protected string TempState;
        protected string ReturnState;
        protected abstract string AppConfigName { get; }
        protected abstract string OAuth2Url { get; }

        protected readonly IMemberService _memberService;

        private readonly string _weixinTesting = ConfigurationManager.AppSettings["Weixin:Testing"];

        private static MemberDTO TestingMember;

        protected static string NotWeiXinManagerMessage = "你没有审批权限";

        protected WeixinBaseController(IMemberService memberService)
        {
            _memberService = memberService;

            TestingMember = new MemberDTO()
            {
                Userid = "zhangshan",
                Name = "张三",
                Position = "主管",
                Departments = new List<DepartmentDTO>() {new DepartmentDTO()
                {
                    DepartmentId = 2,
                    Name = "技术部"
                } }
            };
        }

        public ActionResult NotWeiXinManager()
        {
            return Content(NotWeiXinManagerMessage);
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            this.WeixinBaseInit();
        }

        protected virtual void WeixinBaseInit()
        {
            this.TempState = Guid.NewGuid().ToString().Replace("-", "");
            if (!"true".EqualsIgnoreCase(_weixinTesting))
            {
                this.OAuth2Redirect();
            }

            ViewBag.Member = CurrentMember;
            ViewBag.Department = GetCurrentMemberDepartment();
        }

        protected DepartmentDTO GetCurrentMemberDepartment()
        {
            if (CurrentMember != null 
                && CurrentMember.Departments != null 
                && CurrentMember.Departments.Count > 0)
            {
                return CurrentMember.Departments.First();
            }
            return new DepartmentDTO();
        }

        protected UserDTO GetCurrentOperator()
        {
            return new UserDTO() {Id = Guid.Empty, Name = string.Format("{0}({1})", CurrentMember.Name, CurrentMember.Userid)};
        }

        protected string CurrentUserId
        {
            get
            {
                return Session["Weixin_CurrentUserId"] as string;
            }
            set
            {
                Session["Weixin_CurrentUserId"] = value;
            }
        }

        protected string WeixinRedirectUrl
        {
            get
            {
                return Session["Weixin_RedirectUrl"] as string;
            }
            set
            {
                Session["Weixin_RedirectUrl"] = value;
            }
        }

        private MemberDTO _currentMember;
        
        protected MemberDTO CurrentMember
        {
            get
            {
                if (_currentMember == null)
                {
                    if ("true".EqualsIgnoreCase(_weixinTesting))
                    {
                        _currentMember = TestingMember;
                        CurrentUserId = TestingMember.Userid;
                    }
                    else if (!string.IsNullOrWhiteSpace(CurrentUserId))
                    {
                        _currentMember = _memberService.FindByUserId(CurrentUserId);
                    }
                }

                return _currentMember ?? new MemberDTO();
            }
        }

        protected string GetBaseUrl()
        {
            var baseUrl = string.Format("http://{0}", Request.Url.Host);
            if (Request.Url.Port != 80)
            {
                baseUrl += ":" + Request.Url.Port;
            }

            return baseUrl;
        }

        protected virtual void OAuth2Redirect()
        {
            if (string.IsNullOrWhiteSpace(CurrentUserId))
            {
                this.WeixinRedirectUrl = Request.RawUrl;

                Response.Redirect(string.Format("{0}?state={1}", GetBaseUrl() + OAuth2Url, this.TempState));
                Response.End();
            }
        }
    }
}