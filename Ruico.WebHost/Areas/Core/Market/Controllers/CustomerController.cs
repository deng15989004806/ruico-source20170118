using System.Web.Mvc;

namespace Ruico.WebHost.Areas.Core.Market.Controllers
{
    public class CustomerController : CoreBaseController
    {
        // GET: Market/Customer
        public ActionResult Index()
        {
            return View();
        }
    }
}