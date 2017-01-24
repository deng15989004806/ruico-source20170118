using System.Web.Mvc;

namespace Ruico.WebHost.Areas.Core.Market.Controllers
{
    public class CommerceController : CoreBaseController
    {
        // GET: Market/Commerce
        public ActionResult Index()
        {
            return View();
        }
    }
}