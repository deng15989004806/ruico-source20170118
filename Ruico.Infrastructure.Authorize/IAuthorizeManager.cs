using System;
using Ruico.Infrastructure.Authorize.AuthObject;

namespace Ruico.Infrastructure.Authorize
{
    public interface IAuthorizeManager
    {
        string GetCurrentTokenFromCookies();

        UserForAuthorize GetCurrentUserInfo();

        void SignIn(string loginName, string password, bool rememberMe = false);

        void SignOut();

        void RedirectToLoginPage();

        bool ValidatePermission(string permissionCode, bool throwExceptionIfNotPass = true);

        UserForAuthorize GetAuthorizeUserInfo(UserToken user);

        void ClearAllCache();

        void ClearUserCache(Guid userId);

        bool IsForceLogout { get; set; }
    }
}
