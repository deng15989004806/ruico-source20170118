using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ruico.Application.UserSystemModule;
using Ruico.Dto.UserSystem;
using Ruico.Infrastructure.Authorize;
using Ruico.Infrastructure.Utility.Extensions;
using Ruico.WebHost.Models;
using Ruico.WebHost.Resources.Generated;

namespace Ruico.WebHost.Areas.Core.System.Controllers
{
    [AuthorizeFilter]
    public class RoleController : CoreBaseController
    {
        IRoleService _roleService;

        IRoleGroupService _roleGroupService;

        IMenuService _menuService;

        IUserService _userService;

        IModuleService _moduleService;
        
        #region Constructor

        public RoleController(IRoleService roleService, IRoleGroupService roleGroupService, IMenuService menuService,
            IUserService userService,
            IModuleService moduleService)
        {
            _roleService = roleService;
            _roleGroupService = roleGroupService;
            _menuService = menuService;
            _userService = userService;
            _moduleService = moduleService;
        }

        #endregion

        // GET: UserSystem/Role
        public ActionResult Index(string groupName, int? page)
        {
            var list = _roleGroupService.FindBy(groupName, page.HasValue ? page.Value : 1, CustomDisplayExtensions.DefaultPageSize);

            ViewBag.GroupName = groupName;

            return View(list);
        }

        [HttpPost]
        public ActionResult SearchGroup(string groupName)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index", new
                    {
                        groupName,
                    })
                });
            });
        }

        //[Permission("UserSystem:RoleList")]
        public ActionResult RoleList(Guid groupId, string name, int? page)
        {
            var list = _roleService.FindBy(groupId, name, page.HasValue ? page.Value : 1, CustomDisplayExtensions.DefaultPageSize);

            var group = _roleGroupService.FindBy(groupId);

            ViewBag.GroupName = group == null ? string.Empty : group.Name;
            ViewBag.GroupId = groupId;
            ViewBag.Name = name;

            return View(list);
        }

        [HttpPost]
        public ActionResult SearchRoleList(Guid groupId, string name)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("RoleList", new
                    {
                        groupId,
                        name
                    })
                });
            });
        }
        public ActionResult EditRolePermission(Guid roleId)
        {
            var menus = new List<MenuDTO>();

            var modules = _moduleService.ListAll();
            foreach (var module in modules)
            {
                menus.AddRange(_menuService.FindByModule(module.Id));
            }

            var role = _roleService.FindBy(roleId);
            var roleGroup = _roleGroupService.FindBy(role.RoleGroupId);

            var permissions = _roleService.GetRolePermission(roleId);

            ViewBag.Modules = modules;
            ViewBag.Menus = menus;
            ViewBag.Role = role;
            ViewBag.RoleGroup = roleGroup;
            ViewBag.Permissions = permissions;

            return View();
        }

        [HttpPost]
        public ActionResult EditRolePermission(Guid roleId, List<string> permissions)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                var pList = new List<Guid>();

                foreach (var s in permissions)
                {
                    Guid id;
                    if (Guid.TryParse(s, out id))
                    {
                        pList.Add(id);
                    }
                }

                _roleService.UpdateRolePermission(roleId, pList);

                // 清除用户缓存
                var role = _roleService.FindBy(roleId);
                if (role != null)
                {
                    var users = _roleGroupService.GetUsersIdName(role.RoleGroupId);
                    foreach (var user in users)
                    {
                        AuthorizeManager.ClearUserCache(user.Id);
                    }
                }

                this.JsMessage = MessagesResources.Update_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("EditRolePermission", new
                    {
                        roleId
                    })
                });
            });
        }

        public ActionResult EditUserList(Guid groupId)
        {
            var group = _roleGroupService.FindBy(groupId);

            var allUsers = _userService.GetAllUsersIdName();
            var existsUsers = _roleGroupService.GetUsersIdName(groupId);

            ViewBag.Group = group;
            ViewBag.AllUsers = allUsers;
            ViewBag.ExistsUsers = existsUsers;

            return View();
        }

        [HttpPost]
        public ActionResult EditUserList(Guid groupId, List<string> users)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                var pList = new List<Guid>();

                foreach (var s in users.OpSafe())
                {
                    Guid id;
                    if (Guid.TryParse(s, out id))
                    {
                        pList.Add(id);
                    }
                }

                // 清除用户缓存
                var oldUsers = _roleGroupService.GetUsersIdName(groupId);

                _roleGroupService.UpdateGroupUsers(groupId, pList);

                // 清除用户缓存
                foreach (var u in oldUsers)
                {
                    AuthorizeManager.ClearUserCache(u.Id);
                }
                foreach (var id in pList)
                {
                    AuthorizeManager.ClearUserCache(id);
                }

                this.JsMessage = MessagesResources.Update_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("EditUserList", new
                    {
                        groupId
                    })
                });
            });
        }

        public ActionResult EditRole(Guid groupId, Guid? id)
        {
            var role = id.HasValue ? _roleService.FindBy(id.Value) : new RoleDTO();

            var group = _roleGroupService.FindBy(groupId);

            ViewBag.GroupName = group == null ? string.Empty : group.Name;
            ViewBag.GroupId = groupId;

            return View(role);
        }

        [HttpPost]
        public ActionResult EditRole(Guid groupId, RoleDTO role)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                role.RoleGroupId = groupId;
                if (role.Id == Guid.Empty)
                {
                    _roleService.Add(role);
                    this.JsMessage = MessagesResources.Add_Success;
                }
                else
                {
                    _roleService.Update(role);
                    this.JsMessage = MessagesResources.Update_Success;
                }

                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("RoleList", new {groupId = groupId})
                });
            });
        }

        public ActionResult RemoveRole(Guid groupId, Guid id)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                _roleService.Remove(id);

                this.JsMessage = MessagesResources.Remove_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("RoleList", new {groupId = groupId})
                }, JsonRequestBehavior.AllowGet);
            });
        }

        public ActionResult EditGroup(Guid? id)
        {
            var group = id.HasValue ? _roleGroupService.FindBy(id.Value) : new RoleGroupDTO();
            return View(group);
        }


        [HttpPost]
        public ActionResult EditGroup(RoleGroupDTO roleGroup)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                if (roleGroup.Id == Guid.Empty)
                {
                    _roleGroupService.Add(roleGroup);
                    this.JsMessage = MessagesResources.Add_Success;
                }
                else
                {
                    _roleGroupService.Update(roleGroup);
                    this.JsMessage = MessagesResources.Update_Success;
                }

                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                });
            });
        }

        public ActionResult RemoveGroup(Guid id)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                _roleGroupService.Remove(id);

                this.JsMessage = MessagesResources.Remove_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                }, JsonRequestBehavior.AllowGet);
            });
        }
        
    }
}