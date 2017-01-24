using System.Web.Mvc;

namespace Ruico.WebHost.Areas.Core.Market.Controllers
{
    public class DispatchController : CoreBaseController
    {
        // GET: Market/Dispatch
        public ActionResult Index()
        {
            return View();
        }
    }
}