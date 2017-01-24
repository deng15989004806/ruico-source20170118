using System.Web.Mvc;

namespace Ruico.WebHost
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new CustomAjaxExceptionAttribute(), 1);
            filters.Add(new HandleErrorAttribute(), 2);
        }
    }
}
