using System;
using System.Web.Mvc;
using Ruico.Application.BaseModule;
using Ruico.Dto.Base;
using Ruico.WebHost.Models;
using Ruico.WebHost.Resources.Generated;

namespace Ruico.WebHost.Areas.Core.Base.Controllers
{
    public class ShipController : CoreBaseController
    {
        IShipService _shipService;

        #region Constructor

        public ShipController(IShipService shipService)
        {
            _shipService = shipService;
        }

        #endregion

        /// <summary>
        /// /Base/Ship/Index
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(string name, int? page)
        {
            var list = _shipService.FindBy(name, page.HasValue ? page.Value : 1, CustomDisplayExtensions.DefaultPageSize);

            ViewBag.Name = name;

            return View(list);
        }

        public ActionResult EditShip(Guid? id)
        {
            var ship = id.HasValue
                ? _shipService.FindBy(id.Value)
                : new ShipDTO();

            return View(ship);
        }

        [HttpPost]
        public ActionResult SearchShip(string name)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index", new
                    {
                        name = name,
                    })
                });
            });
        }

        [HttpPost]
        public ActionResult EditShip(ShipDTO ship)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                if (ship.Id == Guid.Empty)
                {
                    ship.CreatorId = GetCurrentUser().UserId;
                    _shipService.Add(ship);
                    this.JsMessage = MessagesResources.Add_Success;
                }
                else
                {
                    _shipService.Update(ship);
                    this.JsMessage = MessagesResources.Update_Success;
                }

                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                });
            });
        }

        public ActionResult RemoveShip(Guid id)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                _shipService.Remove(id);

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