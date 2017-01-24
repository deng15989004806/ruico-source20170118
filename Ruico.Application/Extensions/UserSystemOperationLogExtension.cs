using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ruico.Application.Resources.Generated;
using Ruico.Domain.UserSystemModule.Entities;
using Ruico.Dto.UserSystem;

namespace Ruico.Application.Extensions
{
    public static class UserSystemOperationLogExtension
    {
        #region 角色组

        public static string GetOperationLog(this RoleGroupDTO entity, RoleGroupDTO oldDTO = null)
        {
            var sb = new StringBuilder();

            if (oldDTO == null)
            {
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.Name, entity.Name));
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.Description, entity.Description));
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.SortOrder, entity.SortOrder));
            }
            else
            {
                if (entity.Name != oldDTO.Name)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.Name, oldDTO.Name, entity.Name));
                }
                if (entity.Description != oldDTO.Description)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.Description, oldDTO.Description, entity.Description));
                }
                if (entity.SortOrder != oldDTO.SortOrder)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.SortOrder, oldDTO.SortOrder, entity.SortOrder));
                }
            }

            return sb.ToString().TrimEnd('\r','\n');
        }

        public static string GetOperationLogForUpdateUser(this RoleGroupDTO entity, List<User> users)
        {
            var sb = new StringBuilder();
            users = users ?? new List<User>();
            sb.AppendLine(string.Format("{0}:{1}{2}", UserSystemMessagesResources.User, Environment.NewLine,
                string.Join(Environment.NewLine, users.Select(x => string.Format("{0}({1})", x.Name, x.LoginName)))));

            return sb.ToString().TrimEnd('\r', '\n');
        }

        #endregion

        #region 角色

        public static string GetOperationLog(this RoleDTO entity, RoleDTO oldDTO = null)
        {
            var sb = new StringBuilder();

            if (oldDTO == null)
            {
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.Name, entity.Name));
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.Description, entity.Description));
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.SortOrder, entity.SortOrder));
            }
            else
            {
                if (entity.Name != oldDTO.Name)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.Name, oldDTO.Name, entity.Name));
                }
                if (entity.Description != oldDTO.Description)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.Description, oldDTO.Description, entity.Description));
                }
                if (entity.SortOrder != oldDTO.SortOrder)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.SortOrder, oldDTO.SortOrder, entity.SortOrder));
                }
            }

            return sb.ToString().TrimEnd('\r', '\n');
        }

        public static string GetOperationLogForUpdatePermission(this RoleDTO entity, List<Permission> users)
        {
            var sb = new StringBuilder();
            users = users ?? new List<Permission>();
            sb.AppendLine(string.Format("{0}:{1}{2}", UserSystemMessagesResources.Permission, Environment.NewLine,
                string.Join(Environment.NewLine, users.Select(x => string.Format("{0}>{1}>{2}", x.Menu.Module.Name, x.Menu.Name, x.Name)))));

            return sb.ToString().TrimEnd('\r', '\n');
        }

        #endregion

        #region 用户

        public static string GetOperationLog(this UserDTO entity, UserDTO oldDTO = null)
        {
            var sb = new StringBuilder();

            if (oldDTO == null)
            {
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.Name, entity.Name));
                sb.AppendLine(string.Format("{0}: {1}", UserSystemMessagesResources.LoginName, entity.LoginName));
                sb.AppendLine(string.Format("{0}: {1}", UserSystemMessagesResources.Email, entity.Email));
                sb.AppendLine(string.Format("{0}: {1}", UserSystemMessagesResources.Password, "******"));
            }
            else
            {
                if (entity.Name != oldDTO.Name)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.Name, oldDTO.Name, entity.Name));
                }
                if (entity.LoginName != oldDTO.LoginName)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.Description, oldDTO.LoginName, entity.LoginName));
                }
                if (entity.Email != oldDTO.Email)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.SortOrder, oldDTO.Email, entity.Email));
                }
                if (entity.LoginPwd != oldDTO.LoginPwd)
                {
                    sb.AppendLine(string.Format("{0}: ******", UserSystemMessagesResources.Password));
                }
            }

            return sb.ToString().TrimEnd('\r', '\n');
        }

        public static string GetOperationLogForUpdatePermission(this UserDTO entity, List<Permission> users)
        {
            var sb = new StringBuilder();
            users = users ?? new List<Permission>();
            sb.AppendLine(string.Format("{0}:{1}{2}", UserSystemMessagesResources.Permission, Environment.NewLine,
                string.Join(Environment.NewLine, users.Select(x => string.Format("{0}>{1}>{2}", x.Menu.Module.Name, x.Menu.Name, x.Name)))));

            return sb.ToString().TrimEnd('\r', '\n');
        }

        #endregion

        #region 模块

        public static string GetOperationLog(this ModuleDTO entity, ModuleDTO oldDTO = null)
        {
            var sb = new StringBuilder();

            if (oldDTO == null)
            {
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.Name, entity.Name));
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.SortOrder, entity.SortOrder));
            }
            else
            {
                if (entity.Name != oldDTO.Name)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.Name, oldDTO.Name, entity.Name));
                }
                if (entity.SortOrder != oldDTO.SortOrder)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.SortOrder, oldDTO.SortOrder, entity.SortOrder));
                }
            }

            return sb.ToString().TrimEnd('\r', '\n');
        }

        #endregion

        #region 菜单

        public static string GetOperationLog(this MenuDTO entity, MenuDTO oldDTO = null)
        {
            var sb = new StringBuilder();

            var parentMenus = new List<string>();
            var parent = entity.Parent;
            while (parent != null)
            {
                parentMenus.Add(parent.Name);
                parent = parent.Parent;
            }
            parentMenus.Reverse();
            var parentMenuString = string.Join(" > ", parentMenus.ToArray());

            if (oldDTO == null)
            {
                sb.AppendLine(string.Format("{0}: {1}", UserSystemMessagesResources.Module, entity.Module.Name));
                sb.AppendLine(string.Format("{0}: {1}", UserSystemMessagesResources.Parent_Menu, parentMenuString));
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.Name, entity.Name));
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.Code, entity.Code));
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.Url, entity.Url));
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.SortOrder, entity.SortOrder));
            }
            else
            {
                var oldParentMenus = new List<string>();
                var oldParent = oldDTO.Parent;
                while (oldParent != null)
                {
                    oldParentMenus.Add(oldParent.Name);
                    oldParent = oldParent.Parent;
                }
                oldParentMenus.Reverse();
                var oldParentMenuString = string.Join(" > ", oldParentMenus.ToArray());

                if (entity.Module.Name != oldDTO.Module.Name)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        UserSystemMessagesResources.Module, oldDTO.Module.Name, entity.Module.Name));
                }
                if (parentMenuString != oldParentMenuString)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        UserSystemMessagesResources.Parent_Menu, 
                        parentMenuString, oldParentMenuString));
                }
                if (entity.Name != oldDTO.Name)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.Name, oldDTO.Name, entity.Name));
                }
                if (entity.Code != oldDTO.Code)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.Code, oldDTO.Code, entity.Code));
                }
                if (entity.Url != oldDTO.Url)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.Url, oldDTO.Url, entity.Url));
                }
                if (entity.SortOrder != oldDTO.SortOrder)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.SortOrder, oldDTO.SortOrder, entity.SortOrder));
                }
            }

            return sb.ToString().TrimEnd('\r', '\n');
        }

        #endregion

        #region 权限

        public static string GetOperationLog(this PermissionDTO entity, PermissionDTO oldDTO = null)
        {
            var sb = new StringBuilder();

            if (oldDTO == null)
            {
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.Name, entity.Name));
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.Code, entity.Code));
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.SortOrder, entity.SortOrder));
            }
            else
            {
                if (entity.Name != oldDTO.Name)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.Name, oldDTO.Name, entity.Name));
                }
                if (entity.Code != oldDTO.Code)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.Code, oldDTO.Code, entity.Code));
                }
                if (entity.SortOrder != oldDTO.SortOrder)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.SortOrder, oldDTO.SortOrder, entity.SortOrder));
                }
            }

            return sb.ToString().TrimEnd('\r', '\n');
        }

        #endregion
    }
}
