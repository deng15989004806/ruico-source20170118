using System.Web;
using System.Web.Mvc;
using Ruico.Application.HrModule;
using Ruico.Domain.Weixin.Service;

namespace Ruico.WebHost.Areas.Weixin.KaoQin.Controllers
{
    public class OAuth2Controller : WeixinBaseController
    {
        private ICommonService _commonService;

        public OAuth2Controller(IMemberService memberService,
            ICommonService commonService)
            : base(memberService)
        {
            _commonService = commonService;
        }

        protected override string AppConfigName
        {
            get { return "KaoQin"; }
        }

        protected override string OAuth2Url
        {
            get { return "Weixin/KaoQin/OAuth2"; }
        }

        protected override void OAuth2Redirect()
        {
            //base.OAuth2Redirect();
        }

        [Route("Weixin/KaoQin/OAuth2")]
        public ActionResult Index()
        {
            var redirectUrl = GetBaseUrl() + Url.Action("Callback");
            var state = Request["state"];
            Response.Redirect(_commonService.GetCode(HttpUtility.UrlEncode(redirectUrl), state));
            Response.End();
            return View();
        }

        /// <summary>
        /// OAuth2回调
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [Route("Weixin/KaoQin/OAuth2/Calllback")]
        public ActionResult Callback(string code, string state)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Content("您拒绝了授权！");
            }

            var accessToken = _commonService.GetAccessToken("KaoQin");
            var userId = _commonService.GetUserId(accessToken, code);
            if (!string.IsNullOrWhiteSpace(userId))
            {
                this.CurrentUserId = userId;
            }

            var weixinRedirectUrl = WeixinRedirectUrl;
            if (string.IsNullOrWhiteSpace(weixinRedirectUrl))
            {
                if(Request.UrlReferrer != null)
                {
                    weixinRedirectUrl = Request.UrlReferrer.AbsolutePath;
                }
                else
                {
                    weixinRedirectUrl = Url.Action("Index", "ChuChai");
                }
            }

            var redirectUrl = GetBaseUrl() + weixinRedirectUrl;
            Response.Redirect(redirectUrl);
            Response.End();
            return View();
        }
    }
}