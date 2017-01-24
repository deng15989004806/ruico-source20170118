using System;
using System.Web.Mvc;
using Ruico.Application.BaseModule;
using Ruico.Dto.Base;
using Ruico.WebHost.Models;
using Ruico.WebHost.Resources.Generated;

namespace Ruico.WebHost.Areas.Core.Base.Controllers
{
    public class CargoController : CoreBaseController
    {
        ICargoService _cargoService;

        #region Constructor

        public CargoController(ICargoService cargoService)
        {
            _cargoService = cargoService;
        }

        #endregion

        /// <summary>
        /// /Base/Cargo/Index
        /// </summary>
        /// <param name="name"></param>
        /// <param name="categoryId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(string name, Guid? categoryId, int? page)
        {
            var list = _cargoService.FindBy(name, categoryId, page.HasValue ? page.Value : 1, CustomDisplayExtensions.DefaultPageSize);

            var categories = _cargoService.GetCargoCategories();
            ViewBag.Categories = categories;

            ViewBag.Name = name;
            ViewBag.CategoryId = categoryId;

            return View(list);
        }

        public ActionResult EditCargo(Guid? id)
        {
            var cargo = id.HasValue
                ? _cargoService.FindBy(id.Value)
                : new CargoDTO();

            var categories = _cargoService.GetCargoCategories();
            ViewBag.Categories = categories;

            return View(cargo);
        }

        [HttpPost]
        public ActionResult SearchCargo(string name, Guid? categoryId)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index", new
                    {
                        name = name,
                        categoryId = categoryId,
                    })
                });
            });
        }

        [HttpPost]
        public ActionResult EditCargo(CargoDTO cargo, Guid? categoryId)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                if (categoryId.HasValue)
                {
                    cargo.Category = new CategoryDTO() {Id = categoryId.Value};
                }
                if (cargo.Id == Guid.Empty)
                {
                    cargo.CreatorId = GetCurrentUser().UserId;
                    _cargoService.Add(cargo);
                    this.JsMessage = MessagesResources.Add_Success;
                }
                else
                {
                    _cargoService.Update(cargo);
                    this.JsMessage = MessagesResources.Update_Success;
                }

                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                });
            });
        }

        public ActionResult RemoveCargo(Guid id)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                _cargoService.Remove(id);

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