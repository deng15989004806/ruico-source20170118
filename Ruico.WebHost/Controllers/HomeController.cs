using System.Web.Mvc;
using Ruico.Application.UserSystemModule;
using Ruico.WebHost.Helper;

namespace Ruico.WebHost.Controllers
{
    public class HomeController : BaseAuthorizeController
    {
        IModuleService _moduleService;

        public HomeController()
        {
            _moduleService = RuicoServiceResolver.Resolve<IModuleService>();
        }

        //protected override void CheckLogin()
        //{
        //    //base.CheckoutLogin();
        //}

        public ActionResult Index()
        {
            ViewBag.Modules = _moduleService.ListAll();

            return View();
        }
        public ActionResult Error()
        {
            return View();
        }

        public ActionResult WhatCanIdo()
        {
            return View();
        }
    }
}