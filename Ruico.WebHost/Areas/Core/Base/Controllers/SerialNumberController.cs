using System.Web.Mvc;
using Ruico.Application.SystemModule;
using Ruico.WebHost.Models;
using Ruico.WebHost.Resources.Generated;

namespace Ruico.WebHost.Areas.Core.Base.Controllers
{
    public class SerialNumberController : CoreBaseController
    {
        ISerialNumberService _serialNumberService;

         #region Constructor

        public SerialNumberController(ISerialNumberService serialNumberService)
        {
            _serialNumberService = serialNumberService;
        }

        #endregion

        // GET: Base/SerialNumber
        public ActionResult Index(int? page)
        {
            var list = _serialNumberService.FindBy(page.HasValue ? page.Value : 1, CustomDisplayExtensions.DefaultPageSize);

            return View(list);
        }

        public ActionResult RemoveSerialNumber(long id)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                _serialNumberService.Remove(id);

                this.JsMessage = MessagesResources.Remove_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                }, JsonRequestBehavior.AllowGet);
            });
        }
    }
}