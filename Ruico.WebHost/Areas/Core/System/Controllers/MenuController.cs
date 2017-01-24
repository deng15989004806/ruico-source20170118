using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using Ruico.Application.UserSystemModule;
using Ruico.Dto.UserSystem;
using Ruico.WebHost.Models;
using Ruico.WebHost.Resources.Generated;

namespace Ruico.WebHost.Areas.Core.System.Controllers
{
    public class MenuController : CoreBaseController
    {
        IMenuService _menuService;

        IModuleService _moduleService;

        #region Constructor

        public MenuController(IMenuService menuService,
            IModuleService moduleService)
        {
            _menuService = menuService;
            _moduleService = moduleService;
        }

        #endregion

        public ActionResult Index(Guid? moduleId, string menuName, int? page)
        {
            var list = _menuService.FindBy(moduleId.HasValue ? moduleId.Value : Guid.Empty, menuName,
                page.HasValue ? page.Value : 1, int.MaxValue);

            ViewBag.MenuName = menuName;

            return View(list);
        }

        public ActionResult EditMenu(Guid? id, Guid? moduleId)
        {
            var modules = _moduleService.ListAll();
            ViewBag.Modules = modules;

            ModuleDTO module = null;

            var menu = id.HasValue ? _menuService.FindBy(id.Value) : new MenuDTO();
            if (menu.Id != Guid.Empty && menu.Module != null)
            {
                module = menu.Module;
            }

            if (moduleId.HasValue)
            {
                module = modules.FirstOrDefault(x => x.Id == moduleId.Value);
            }

            ViewBag.Module = module ?? new ModuleDTO();

            ViewBag.Menus = _menuService.FindBy(module == null ? Guid.Empty : module.Id, string.Empty, 1, int.MaxValue).ToList();

            return View(menu);
        }

        [HttpPost]
        public ActionResult EditMenu(MenuDTO menu, Guid? module, Guid? parent)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                menu.Permissions = new Collection<PermissionDTO>();
                if (module.HasValue)
                {
                    menu.Module = _moduleService.FindBy(module.Value);
                }
                if (parent.HasValue)
                {
                    menu.Parent = _menuService.FindBy(parent.Value);
                }

                if (menu.Id == Guid.Empty)
                {
                    _menuService.Add(menu);
                    this.JsMessage = MessagesResources.Add_Success;
                }
                else
                {
                    _menuService.Update(menu);
                    this.JsMessage = MessagesResources.Update_Success;
                }
                base.ClearCacheMenus();
                // 更新所有登陆用户缓存，以更新菜单信息
                AuthorizeManager.ClearAllCache();

                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                });
            });
        }

        public ActionResult RemoveMenu(Guid id)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                _menuService.Remove(id);
                base.ClearCacheMenus();
                // 更新所有登陆用户缓存，以更新菜单信息
                AuthorizeManager.ClearAllCache();

                this.JsMessage = MessagesResources.Remove_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                }, JsonRequestBehavior.AllowGet);
            });
        }

        [HttpPost]
        public ActionResult SearchMenu(string menuName)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index", new
                    {
                        menuName,
                    })
                });
            });
        }

        public ActionResult MenuPermissionList(Guid menuId)
        {
            var menu = _menuService.FindBy(menuId) ?? new MenuDTO();

            var list =  menu.Permissions;

            ViewBag.MenuName = menu.Name;
            ViewBag.MenuId = menuId;

            return View(list);
        }

        public ActionResult EditMenuPermission(Guid menuId, Guid? id)
        {
            var menu = _menuService.FindBy(menuId) ?? new MenuDTO();
            var permission = id.HasValue ?
                menu.Permissions.FirstOrDefault(x => x.Id == id) : new PermissionDTO();

            ViewBag.MenuName = menu.Name;
            ViewBag.MenuId = menuId;

            return View(permission);
        }

        [HttpPost]
        public ActionResult EditMenuPermission(Guid menuId, PermissionDTO permission, Guid? id)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                var menu = _menuService.FindBy(menuId) ?? new MenuDTO();
                if (!id.HasValue)
                {
                    menu.Permissions.Add(permission);
                }
                else
                {
                    permission.Id = id.Value;
                    var oldPermission = menu.Permissions.FirstOrDefault(x => x.Id == permission.Id);
                    if (oldPermission != null)
                    {
                        permission.Created = oldPermission.Created;
                        menu.Permissions.Remove(oldPermission);
                    }
                    menu.Permissions.Add(permission);
                }
                _menuService.UpdatePermission(menu);

                this.JsMessage = MessagesResources.Update_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("MenuPermissionList", new {menuId = menuId})
                });
            });
        }

        public ActionResult RemoveMenuPermission(Guid menuId, Guid id)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                var menu = _menuService.FindBy(menuId) ?? new MenuDTO();
                var permission = menu.Permissions.FirstOrDefault(x => x.Id == id);
                if (permission != null)
                {
                    menu.Permissions.Remove(permission);
                    _menuService.UpdatePermission(menu);
                }

                this.JsMessage = MessagesResources.Remove_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("MenuPermissionList", new {menuId = menuId})
                }, JsonRequestBehavior.AllowGet);
            });
        }

        //
        // GET: /UserSystem/User/
        //public ActionResult Index()
        //{
        //    return View();
        //}
    }
}