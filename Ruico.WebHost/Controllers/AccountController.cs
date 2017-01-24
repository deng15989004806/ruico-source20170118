using System;
using System.Web.Mvc;
using Ruico.Application.Exceptions;
using Ruico.Infrastructure.Authorize;
using Ruico.WebHost.Models;
using Ruico.WebHost.Resources.Generated;

namespace Ruico.WebHost.Controllers
{
    public class AccountController : BaseController
    {
        IAuthorizeManager AuthorizeManager;

        public AccountController(IAuthorizeManager authorizeManager)
        {
            AuthorizeManager = authorizeManager;
        }

        public ActionResult Login()
        {
            var returnUrl = Request["ReturnUrl"] ?? "/";
            if (returnUrl.IndexOf("Logout", StringComparison.OrdinalIgnoreCase) > -1)
            {
                returnUrl = "/";
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public ActionResult Forbidden()
        {
            return View();
        }

        public ActionResult Logout()
        {
            AuthorizeManager.SignOut();
            AuthorizeManager.RedirectToLoginPage();
            return null;
        }

        [HttpPost]
        public ActionResult SignIn(string returnUrl, string loginName, string password, bool? rememberMe)
        {
            var response = new AjaxResponse();
            if (loginName.Trim() == "" || password.Trim() == "")
            {
                response.Succeeded = false;
                response.ErrorMessage = "用户名或密码不能为空。";
                return Json(response);
            }

            try
            {
                AuthorizeManager.SignIn(loginName, password, rememberMe.HasValue && rememberMe.Value);
                response.Succeeded = true;
                response.RedirectUrl = returnUrl;
                response.ShowMessage = true;
                response.Message = MessagesResources.Login_Success;
            }
            catch (Exception ex)
            {
                if (!(ex is DefinedException))
                {
                    Log.Error(ex.GetIndentedExceptionLog());
                }
                response.Succeeded = false;
                response.ErrorMessage = ex.Message;
                response.ShowMessage = true;
            }
            return  Json(response);
        }
    }
}