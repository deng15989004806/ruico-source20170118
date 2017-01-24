using System;
using System.Collections.Generic;
using System.Linq;
using Ruico.Application.Exceptions;
using Ruico.Application.Resources.Generated;
using Ruico.Domain.UserSystemModule.Repositories;
using Ruico.Dto.UserSystem;
using Ruico.Dto.UserSystem.Converters;
using Ruico.Infrastructure.Utility.Helper;

namespace Ruico.Application.UserSystemModule.Imp
{
    public class AuthService : IAuthService
    {
        #region 私有成员

        private IUserRepository _userRepository;

        private void UpdateLoginToken(Guid id, string loginToken, DateTime lastLoginTime)
        {
            var user = _userRepository.Get(id);

            if (user == null)
            {
                throw new ArgumentException(id.ToString(), "id");
            }

            user.LastLoginToken = loginToken;
            user.LastLogin = lastLoginTime;

            //commit unit of work
            _userRepository.UnitOfWork.Commit();
        }

        #endregion

        #region Constructors

        public AuthService(IUserRepository userRepository)                               
        {
            if (userRepository == null)
                throw new ArgumentNullException("userRepository");

            _userRepository = userRepository;
        }

        #endregion

        public UserDTO Login(string loginName, string password, bool updateLoginToken)
        {
            if (loginName.IsNullOrBlank())
            {
                throw new ArgumentEmptyException(UserSystemMessagesResources.LoginName_Empty);
            }

            if (password.IsNullOrBlank())
            {
                throw new ArgumentEmptyException(UserSystemMessagesResources.Password_Empty);
            }

            var user = _userRepository.Find(x => x.LoginName.Equals(loginName));

            if (user == null)
            {
                throw new DataNotFoundException(string.Format(UserSystemMessagesResources.LoginName_NotExists_WithValue, loginName));
            }

            if (!EncryptPassword(password).EqualsIgnoreCase(user.LoginPwd))
            {
                throw new DefinedException(UserSystemMessagesResources.Password_Incorrect);
            }

            if (updateLoginToken)
            {
                var newToken = SecurityHelper.NetxtString(24).ToLower();
                UpdateLoginToken(user.Id, newToken, DateTime.UtcNow);
                user.LastLoginToken = newToken;
            }

            if (user.LastLoginToken.IsNullOrBlank())
            {
                throw new DefinedException(UserSystemMessagesResources.LoginToken_Empty);
            }

            return user.ToDto();
        }

        public bool ValidatePassword(Guid id, string password)
        {
            var user = _userRepository.Get(id);

            if (user == null)
            {
                throw new ArgumentException(id.ToString(), "id");
            }

            return EncryptPassword(password).EqualsIgnoreCase(user.LoginPwd);
        }

        public void ChangePassword(Guid id, string password)
        {
            var user = _userRepository.Get(id);

            if (user == null)
            {
                throw new ArgumentException(id.ToString(), "id");
            }

            user.LoginPwd = EncryptPassword(password);

            //commit unit of work
            _userRepository.UnitOfWork.Commit();
        }

        public bool ValidateLoginToken(Guid id, string token)
        {
            if (token.IsNullOrBlank())
            {
                throw new ArgumentException(token, "token");
            }

            var user = _userRepository.Get(id);

            if (user == null)
            {
                throw new ArgumentException(id.ToString(), "id");
            }

            return token.EqualsIgnoreCase(user.LastLoginToken);
        }

        public UserDTO FindByLoginToken(Guid id, string token)
        {
            var user = _userRepository.Get(id);

            if (user == null)
            {
                throw new ArgumentException(id.ToString(), "id");
            }

            user.LastLoginToken = user.LastLoginToken ?? string.Empty;

            if (!user.LastLoginToken.EqualsIgnoreCase(token))
            {
                throw new DataNotFoundException(UserSystemMessagesResources.LoginToken_Invalid);
            }

            return user.ToDto();
        }

        public List<PermissionForAuthDTO> GetRolePermissions(Guid id)
        {
            var user = _userRepository.Get(id);

            if (user == null)
            {
                throw new ArgumentException(id.ToString(), "id");
            }

            var permissions = new List<PermissionForAuthDTO>();

            var roles = user.Groups.Where(g => g.Roles != null).SelectMany(x => x.Roles);

            foreach (var role in roles.Where(role => role.Permissions != null))
            {
                permissions.AddRange(role.Permissions.Select(x => new PermissionForAuthDTO()
                {
                    PermissionId = x.Id,
                    PermissionCode = x.Code,
                    MenuId = x.Menu.Id,
                    RoleId = role.Id,
                    PermissionName = x.Name,
                    MenuName = x.Menu.Name,
                    MenuUrl = x.Menu.Url,
                    RoleName = role.Name,
                    PermissionSortOrder = x.SortOrder,
                    MenuSortOrder = x.Menu.SortOrder,
                    ModuleId = x.Menu.Module.Id,
                    ModuleName = x.Menu.Module.Name,
                    MenuDepth = x.Menu.Depth,
                    ParentMenuId = x.Menu.Parent == null ? Guid.Empty : x.Menu.Parent.Id
                }));

                // 添加上级的菜单
                foreach (var menu in role.Permissions.Where(x => x.Menu.Parent != null).Select(x => x.Menu))
                {
                    var parent = menu.Parent;
                    while (parent != null)
                    {
                        permissions.Add(new PermissionForAuthDTO()
                        {
                            MenuId = parent.Id,
                            RoleId = role.Id,
                            PermissionId = Guid.Empty,
                            PermissionName = string.Empty,
                            MenuName = parent.Name,
                            MenuUrl = parent.Url,
                            RoleName = role.Name,
                            MenuSortOrder = parent.SortOrder,
                            ModuleId = parent.Module.Id,
                            ModuleName = parent.Module.Name,
                            MenuDepth = parent.Depth,
                            ParentMenuId = parent.Parent == null ? Guid.Empty : parent.Parent.Id
                        });
                        parent = parent.Parent;
                    }
                }
            }

            return permissions;
        }

        public List<PermissionForAuthDTO> GetUserPermissions(Guid id)
        {
            var user = _userRepository.Get(id);

            if (user == null)
            {
                throw new ArgumentException(id.ToString(), "id");
            }

            var permissions = user.Permissions != null ? user.Permissions.Select(x => new PermissionForAuthDTO()
            {
                PermissionId = x.Id,
                PermissionCode = x.Code,
                MenuId = x.Menu.Id,
                FromUser = true,
                PermissionName = x.Name,
                MenuName = x.Menu.Name,
                MenuUrl = x.Menu.Url,
                PermissionSortOrder = x.SortOrder,
                MenuSortOrder = x.Menu.SortOrder,
                ModuleId = x.Menu.Module.Id,
                ModuleName = x.Menu.Module.Name,
                MenuDepth = x.Menu.Depth,
                ParentMenuId = x.Menu.Parent == null ? Guid.Empty : x.Menu.Parent.Id
            }).ToList()
                : new List<PermissionForAuthDTO>();

            // 添加上级的菜单
            foreach (var menu in user.Permissions.Where(x => x.Menu.Parent != null).Select(x => x.Menu))
            {
                var parent = menu.Parent;
                while (parent != null)
                {
                    permissions.Add(new PermissionForAuthDTO()
                    {
                        MenuId = parent.Id,
                        PermissionId = Guid.Empty,
                        PermissionName = string.Empty,
                        FromUser = true,
                        MenuName = parent.Name,
                        MenuUrl = parent.Url,
                        MenuSortOrder = parent.SortOrder,
                        ModuleId = parent.Module.Id,
                        ModuleName = parent.Module.Name,
                        MenuDepth = parent.Depth,
                        ParentMenuId = parent.Parent == null ? Guid.Empty : parent.Parent.Id
                    });
                    parent = parent.Parent;
                }
            }

            return permissions;
        }

        public static string EncryptPassword(string password)
        {
            return SecurityHelper.EncryptPassword(password);
        }
    }
}
