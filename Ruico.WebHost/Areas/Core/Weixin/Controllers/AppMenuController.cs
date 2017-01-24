using System;
using System.Linq;
using System.Web.Mvc;
using Ruico.Application.WeixinModule;
using Ruico.Domain.Weixin.Service;
using Ruico.Dto.Weixin;
using Ruico.WebHost.Models;
using Ruico.WebHost.Resources.Generated;

namespace Ruico.WebHost.Areas.Core.Weixin.Controllers
{
    public class AppMenuController : CoreBaseController
    {
        IAppMenuService _appMenuService;
        IMenuService _menuService;
        ICommonService _commonService;

        #region Constructor

        public AppMenuController(IAppMenuService appMenuService,
            IMenuService menuService,
            ICommonService commonService)
        {
            _appMenuService = appMenuService;
            _menuService = menuService;
            _commonService = commonService;
        }

        #endregion

        /// <summary>
        /// /Core/Weixin/AppMenu/Index
        /// </summary>
        /// <param name="name"></param>
        /// <param name="appId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(string name, int? appId, int? page)
        {
            var list = _appMenuService.FindBy(appId, name, page.HasValue ? page.Value : 1, int.MaxValue);

            var apps = _appMenuService.GetApps();
            ViewBag.Apps = apps;

            ViewBag.Name = name;
            ViewBag.AppId = appId;

            return View(list);
        }

        [HttpPost]
        public ActionResult SearchAppMenu(string name, int? appId)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index", new
                    {
                        name = name,
                        appId = appId,
                    })
                });
            });
        }

        public ActionResult EditAppMenu(Guid? id, int? appId)
        {
            var appMenu = id.HasValue
                ? _appMenuService.FindBy(id.Value)
                : new AppMenuDTO();

            var apps = _appMenuService.GetApps();
            ViewBag.Apps = apps;
            ViewBag.AppId = appId.HasValue ? appId.Value : appMenu.AppId;

            ViewBag.Menus =
                _appMenuService.FindBy(appId.HasValue ? appId.Value : appMenu.AppId, string.Empty, 1, int.MaxValue)
                    .ToList();

            return View(appMenu);
        }

        [HttpPost]
        public ActionResult EditAppMenu(AppMenuDTO appMenu, Guid? parent)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                if (parent.HasValue)
                {
                    appMenu.Parent = _appMenuService.FindBy(parent.Value);
                }

                if (appMenu.Id == Guid.Empty)
                {
                    _appMenuService.Add(appMenu);
                    this.JsMessage = MessagesResources.Add_Success;
                }
                else
                {
                    _appMenuService.Update(appMenu);
                    this.JsMessage = MessagesResources.Update_Success;
                }

                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                });
            });
        }

        public ActionResult RemoveAppMenu(Guid id)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                _appMenuService.Remove(id);

                this.JsMessage = MessagesResources.Remove_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                }, JsonRequestBehavior.AllowGet);
            });
        }

        public ActionResult GetMenusFromWeixin(int appId)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                var apps = _commonService.GetApps();
                var app = apps.FirstOrDefault(x => x.Value == appId.ToString());

                if (app == null)
                {
                    throw new Exception("app not found. appId:" + appId);
                }

                var accessToken = _commonService.GetAccessToken(app.Key);
                var menus = _menuService.GetMenus(accessToken, appId.ToString());

                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    Data = new {menus}
                }, JsonRequestBehavior.AllowGet);
            });
        }

        public ActionResult DownloadMenusFromWeixin(int appId)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                _appMenuService.DownloadMenus(appId);

                this.JsMessage = MessagesResources.Download_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                }, JsonRequestBehavior.AllowGet);
            });
        }

        public ActionResult UploadMenusToWeixin(int appId)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                _appMenuService.UploadMenus(appId);

                this.JsMessage = MessagesResources.Upload_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                }, JsonRequestBehavior.AllowGet);
            });
        }
    }
}