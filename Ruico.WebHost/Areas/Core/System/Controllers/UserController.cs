using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ruico.Application.UserSystemModule;
using Ruico.Dto.UserSystem;
using Ruico.Infrastructure.Authorize.AuthObject;
using Ruico.Infrastructure.Utility.Extensions;
using Ruico.WebHost.Models;
using Ruico.WebHost.Resources.Generated;

namespace Ruico.WebHost.Areas.Core.System.Controllers
{
    public class UserController : CoreBaseController
    {
        IUserService _userService;

        IMenuService _menuService;

        IModuleService _moduleService;

        #region Constructor

        public UserController(IUserService userService, 
            IMenuService menuService,
            IModuleService moduleService)
        {
            _userService = userService;
            _menuService = menuService;
            _moduleService = moduleService;
        }

        #endregion

        public ActionResult Index(string userName, int? page)
        {
            var list = _userService.FindBy(userName, page.HasValue ? page.Value : 1, CustomDisplayExtensions.DefaultPageSize);

            ViewBag.UserName = userName;

            return View(list);
        }

        public ActionResult EditUser(Guid? id)
        {
            var user = id.HasValue ? _userService.FindBy(id.Value) : new UserDTO();
            return View(user);
        }

        [HttpPost]
        public ActionResult SearchUser(string userName)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index", new
                    {
                        userName,
                    })
                });
            });
        }

        public ActionResult EditUserPermission(Guid userId)
        {
            var menus = new List<MenuDTO>();

            var modules = _moduleService.ListAll();
            foreach (var module in modules)
            {
                menus.AddRange(_menuService.FindByModule(module.Id));
            }

            var user = _userService.FindBy(userId);

            var permissions = _userService.GetUserPermission(userId);

            ViewBag.Modules = modules;
            ViewBag.Menus = menus;
            ViewBag.User = user;
            ViewBag.Permissions = permissions;

            return View();
        }

        [HttpPost]
        public ActionResult EditUserPermission(Guid userId, List<string> permissions)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                var pList = new List<Guid>();

                foreach (var s in permissions.OpSafe())
                {
                    Guid id;
                    if (Guid.TryParse(s, out id))
                    {
                        pList.Add(id);
                    }
                }

                _userService.UpdateUserPermission(userId, pList);
                AuthorizeManager.ClearUserCache(userId);

                this.JsMessage = MessagesResources.Update_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("EditUserPermission", new
                    {
                        userId
                    })
                });
            });
        }

        [HttpPost]
        public ActionResult EditUser(UserDTO user)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                if (user.LastLogin == DateTime.MinValue) //最后登录时间字段为空时，数据为datetime默认的{0001/1/1 0:00:00}，新增或修改用户时报错
                    user.LastLogin = Convert.ToDateTime("1900-01-01T00:00:00.000");

                if (user.Id == Guid.Empty)
                {
                    _userService.Add(user);
                    this.JsMessage = MessagesResources.Add_Success;
                }
                else
                {
                    _userService.Update(user);
                    this.JsMessage = MessagesResources.Update_Success;
                }
                AuthorizeManager.ClearUserCache(user.Id);

                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                });
            });
        }

        public ActionResult RemoveUser(Guid id)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                _userService.Remove(id);
                AuthorizeManager.ClearUserCache(id);

                this.JsMessage = MessagesResources.Remove_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                }, JsonRequestBehavior.AllowGet);
            });
        }

        public ActionResult EffectiveUserPermission(Guid userId)
        {
            var user = _userService.FindBy(userId);

            ViewBag.AuthorizeUser = AuthorizeManager.GetAuthorizeUserInfo(
                new UserToken() { UserId = user.Id, LastLoginToken = user.LastLoginToken });

            return View();
        }
    }
}