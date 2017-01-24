using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Ruico.Application.SystemModule;
using Ruico.Application.UserSystemModule;
using Ruico.Dto.UserSystem;
using Ruico.Infrastructure.Authorize;
using Ruico.Infrastructure.Authorize.AuthObject;
using Ruico.Infrastructure.Utility.Caching;
using Ruico.Infrastructure.Utility.Extensions;
using Ruico.Infrastructure.Utility.Helper;
using Ruico.WebHost.Resources.Generated;

namespace Ruico.WebHost.Helper
{
    public class AuthorizeManager : IAuthorizeManager
    {
        private static ConcurrentDictionary<string, string> CacheKeys = new ConcurrentDictionary<string, string>();
        private static ConcurrentDictionary<string, string> UserTokens = new ConcurrentDictionary<string, string>();

        private ICacheManager CacheManager { get; set; }

        private IAuthService AuthService { get; set; }

        private IOperateLogService OperateLogService { get; set; }

        /// <summary>
        /// 强制退出
        /// </summary>
        public bool IsForceLogout { get; set; }

        public AuthorizeManager(ICacheManager cacheManager, IAuthService authService,
            IOperateLogService operateLogService)
        {
            CacheManager = cacheManager;
            AuthService = authService;
            OperateLogService = operateLogService;
        }

        #region Private Methods

        private static string GetTokenKey(string token)
        {
            return string.Format("RUICO_AUTH_TOKEN_{0}", token);
        }

        private void RegisterToken(string token, string user = "RuicoUser", bool rememberMe = false)
        {
            if (token.IsNullOrBlank())
            {
                throw new ArgumentNullException("token", token);
            }

            DateTime expiration = rememberMe ? DateTime.Now.AddDays(7) : DateTime.Now.Add(FormsAuthentication.Timeout);

            var ticket = new FormsAuthenticationTicket(1, user, DateTime.Now, expiration, true, token, FormsAuthentication.FormsCookiePath);

            string hashTicket = FormsAuthentication.Encrypt(ticket);
            var userCookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashTicket) { Domain = FormsAuthentication.CookieDomain };
            if (rememberMe)
            {
                userCookie.Expires = expiration;
            }

            HttpContext.Current.Response.Cookies.Set(userCookie);
        }

        private void RemoveToken(string token, UserDTO user = null)
        {
            CacheManager.Remove(GetTokenKey(token));
            if (user != null)
            {
                string str;
                UserTokens.TryRemove(user.Id.ToString(), out str);
            }
        }

        private void SetCacheAuthUser(string authToken, UserForAuthorize authUser)
        {
            var key = GetTokenKey(authToken);
            // 缓存起来
            CacheManager.Set(key, authUser, 12 * 60);

            CacheKeys.TryAdd(key, key);
            UserTokens.TryAdd(authUser.UserId.ToString(), authToken);
        }

        private UserForAuthorize GetAuthorizeUserInfo(string token, bool validateLoginToken = true)
        {
            var authUser = CacheManager.Get<UserForAuthorize>(GetTokenKey(token));

            var datas = token.Split('_');
            var loginToken = string.Empty;
            if (datas.Length == 2)
            {
                var userId = Guid.Parse(datas[0]);
                loginToken = datas[1];
                if (authUser == null)
                {
                    // 尝试从数据库读取
                    var user = AuthService.FindByLoginToken(userId, loginToken);
                    if (user != null)
                    {
                        authUser = GetAuthUserFromDb(user);
                        // 写入到缓存
                        SetCacheAuthUser(token, authUser);
                    }
                }
            } 
            
            if (authUser == null)
            {
                return null;
            }

            if (validateLoginToken)
            {
                if (!AuthService.ValidateLoginToken(authUser.UserId, loginToken))
                {
                    IsForceLogout = true;
                    return null;
                }
            }

            return authUser;
        }

        private UserForAuthorize GetAuthUserFromDb(UserDTO user)
        {
            var authUser = new UserForAuthorize()
            {
                LoginName = user.LoginName,
                UserId = user.Id,
                UserName = user.Name
            };

            // 权限和菜单信息
            var permissions = new List<PermissionForAuthDTO>();
            permissions.AddRange(AuthService.GetRolePermissions(user.Id));
            permissions.AddRange(AuthService.GetUserPermissions(user.Id));

            authUser.Permissions = permissions.Where(x=>x.PermissionName.NotNullOrBlank())
                .Select(x => new PermissionForAuthorize()
                {
                    PermissionId = x.PermissionId,
                    PermissionCode = x.PermissionCode,
                    MenuId = x.MenuId,
                    RoleId = x.RoleId,
                    FromUser = x.FromUser,
                    PermissionName = x.PermissionName,
                    MenuName = x.MenuName,
                    MenuUrl = x.MenuUrl,
                    RoleName = x.RoleName,
                    PermissionSortOrder = x.PermissionSortOrder,
                    MenuSortOrder = x.MenuSortOrder,
                    ModuleId = x.ModuleId,
                    ModuleName = x.ModuleName
                }).OrderBy(x => x.ModuleId).ThenBy(x => x.MenuSortOrder)
                .ThenBy(x => x.PermissionSortOrder).ToList();
            authUser.Menus = permissions.Select(x => new MenuForAuthorize()
            {
                ModuleId = x.ModuleId,
                ModuleName = x.ModuleName,
                MenuId = x.MenuId,
                MenuName = x.MenuName,
                MenuUrl = x.MenuUrl,
                MenuDepth = x.MenuDepth,
                ParentMenuId = x.ParentMenuId,
                MenuSortOrder = x.MenuSortOrder
            }).DistinctBy(x => x.MenuId).ToList();

            return authUser;
        }
        #endregion

        public string GetCurrentTokenFromCookies()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var token = ((FormsIdentity)HttpContext.Current.User.Identity).Ticket.UserData;
                if (token.NotNullOrBlank())
                {
                    return token;
                }
                throw new AuthorizeTokenNotFoundException();
            }
            throw new AuthorizeTokenNotFoundException();
        }

        public UserForAuthorize GetCurrentUserInfo()
        {
            var token = this.GetCurrentTokenFromCookies();

            return GetAuthorizeUserInfo(token);
        }

        public UserForAuthorize GetAuthorizeUserInfo(UserToken user)
        {
            var token = user.GetAuthToken();

            return GetAuthorizeUserInfo(token, false);
        }

        public void ClearAllCache()
        {
            foreach (var cacheKey in CacheKeys)
            {
                CacheManager.Remove(cacheKey.Key);
            }
        }

        public void ClearUserCache(Guid userId)
        {
            if (UserTokens.ContainsKey(userId.ToString()))
            {
                RemoveToken(UserTokens[userId.ToString()]);
            }
        }

        public void SignIn(string loginName, string password, bool rememberMe = false)
        {
            var user = AuthService.Login(loginName, password, true);

            var authUser = GetAuthUserFromDb(user);

            var dataToken = new UserToken() {UserId = user.Id, LastLoginToken = user.LastLoginToken}.GetAuthToken();

            // Cache
            SetCacheAuthUser(dataToken, authUser);

            // Cookies
            RegisterToken(dataToken, authUser.UserName, rememberMe);

            #region 操作日志

            OperateLogService.RecordOperation(user.Id.ToString(),
                string.Format("{0}", MessagesResources.Login),
                MessagesResources.Login_Success,
                user,
                true);

            #endregion
        }

        public void SignOut()
        {
            try
            {
                var token = this.GetCurrentTokenFromCookies();

                UserDTO user = null;
                try
                {
                    var authUser = GetCurrentUserInfo();
                    if (authUser != null)
                    {
                        user = new UserDTO()
                        {
                            Id = authUser.UserId,
                            Name = authUser.UserName,
                            LoginName = authUser.LoginName
                        };
                    }
                }
                catch (Exception ex)
                {
                    //throw;
                }

                RemoveToken(token, user);

                FormsAuthentication.SignOut();

                if (user != null)
                {
                    #region 操作日志

                    OperateLogService.RecordOperation(user.Id.ToString(),
                        string.Format("{0}", MessagesResources.Logout),
                        MessagesResources.Logout_Success,
                        user,
                        true);

                    #endregion
                }
            }
            catch (AuthorizeTokenNotFoundException)
            {
                return;
            }
        }

        public void RedirectToLoginPage()
        {
            FormsAuthentication.RedirectToLoginPage();
        }

        public bool ValidatePermission(string permissionCode, bool throwExceptionIfNotPass = true)
        {
            var user = GetCurrentUserInfo();
            if (user == null)
            {
                throw new AuthorizeTokenInvalidException();
            }

            var permission = user.Permissions.FirstOrDefault(x => x.PermissionCode.EqualsIgnoreCase(permissionCode));

            if (permission == null && throwExceptionIfNotPass)
            {
                throw new AuthorizeNoPermissionException();
            }

            return permission != null;
        }
    }
}
