using System.Web.Mvc;

namespace Ruico.WebHost.Areas.Weixin.KaoQin
{
    public class WeixinAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Weixin/KaoQin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Weixin_KaoQin_default",
                "Weixin/KaoQin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}