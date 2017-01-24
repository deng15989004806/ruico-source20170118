using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Ruico.Application.Exceptions;
using Ruico.Application.UserSystemModule;
using Ruico.Dto.UserSystem;
using Ruico.Infrastructure.Authorize;
using Ruico.Infrastructure.Authorize.AuthObject;
using Ruico.Infrastructure.Utility.Caching;
using Ruico.Infrastructure.Utility.Helper;
using Ruico.WebHost.Helper;
using Ruico.WebHost.Models.System;
using Ruico.WebHost.Resources.Generated;

namespace Ruico.WebHost.Controllers
{
    public class BaseAuthorizeController : BaseController
    {
        //public IServiceResolver ServiceResolver { get; set; }

        protected IAuthorizeManager AuthorizeManager
        {
            get
            {
                return RuicoServiceResolver.Resolve<IAuthorizeManager>();
            }
        }

        IModuleService ModuleService;
        IMenuService MenuService;
        ICacheManager CacheManager;

        private static int CacheInMinutes = 12 * 60;
        private static string CacheKey_Prex = "Ruico_CacheKey_";
        private static string CacheKey_Modules = CacheKey_Prex + "Modules";
        private static string CacheKey_Menus = CacheKey_Prex + "Menus";

        public List<ModuleDTO> CacheModules
        {
            get
            {
                return CacheManager.Get(CacheKey_Modules, CacheInMinutes, () => ModuleService.ListAll());
            }
        }
        public List<MenuDTO> CacheMenus
        {
            get
            {
                return CacheManager.Get(CacheKey_Menus, CacheInMinutes,
                    () => MenuService.FindBy(Guid.Empty, null, 1, int.MaxValue).ToList());
            }
        }

        public void ClearCacheModules()
        {
            CacheManager.Remove(CacheKey_Modules);
        }
        public void ClearCacheMenus()
        {
            CacheManager.Remove(CacheKey_Menus);
        }

        private UserForAuthorize _CurrentUser;

        public BaseAuthorizeController()
        {
            ModuleService = RuicoServiceResolver.Resolve<IModuleService>();
            MenuService = RuicoServiceResolver.Resolve<IMenuService>();
            CacheManager = RuicoServiceResolver.Resolve<ICacheManager>();

            var currentUser = this.GetCurrentUser();
            ViewBag.CurrentUser = currentUser;
            ViewBag.Modules = this.CacheModules;

            var extModules = new List<ExtModuleDTO>();
            if (currentUser != null)
            {
                var userModuleIds = currentUser.Menus.Select(x => x.ModuleId).Distinct();
                extModules = this.CacheModules
                    .Where(x => userModuleIds.Contains(x.Id))
                    .Select(x => new ExtModuleDTO()
                    {
                        Module = x,
                        Url = GetModuleUrlByMenus(x.Id, this.CacheMenus)
                    }).ToList();
            }
            ViewBag.ExtModules = extModules;
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var currentPath = Request.Url.AbsolutePath;
            ViewBag.CurrentPath = currentPath;

            var currentModuleId = string.Empty;
            if (_CurrentUser != null)
            {
                var menu =
                    _CurrentUser.Menus.FirstOrDefault(
                        x => x.MenuUrl.NotNullOrBlank() && 
                            currentPath.ToLower().Contains(x.MenuUrl.TrimEnd('/').ToLower()));
                if (menu != null)
                {
                    currentModuleId = menu.ModuleId.ToString();
                }
            }
            ViewBag.CurrentModuleId = currentModuleId;
        }

        /// <summary>
        /// 找到某模块下，第一个菜单的Url，作为模块的Url
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="menus"></param>
        /// <returns></returns>
        private static string GetModuleUrlByMenus(Guid moduleId, IEnumerable<MenuDTO> menus)
        {
            var moduleMenus = menus.Where(x => x.Module.Id == moduleId && x.Url.NotNullOrBlank()).OrderBy(x => x.SortOrder);

            var menu = moduleMenus.FirstOrDefault();

            return menu == null ? string.Empty : menu.Url;
        }

        protected UserForAuthorize GetCurrentUser()
        {
            try
            {
                _CurrentUser = AuthorizeManager.GetCurrentUserInfo();
            }
            catch (ArgumentException)
            {
            }
            catch (DataNotFoundException)
            {
            }
            catch (AuthorizeTokenNotFoundException)
            {
            }
            return _CurrentUser;
        }

        protected virtual void CheckLogin()
        {
            if (GetCurrentUser() == null)
            {
                if (AuthorizeManager.IsForceLogout)
                {
                    this.JsMessage = MessagesResources.ForceLogout_Success;
                    AuthorizeManager.IsForceLogout = false;
                }
                AuthorizeManager.RedirectToLoginPage();
            }
            
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            CheckLogin();
        }
    }
}