using System;
using System.Web.Mvc;
using Ruico.Application.UserSystemModule;
using Ruico.Dto.UserSystem;
using Ruico.WebHost.Models;
using Ruico.WebHost.Resources.Generated;

namespace Ruico.WebHost.Areas.Core.System.Controllers
{
    public class ModuleController : CoreBaseController
    {
        IModuleService _moduleService;

        #region Constructor

        public ModuleController(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }

        #endregion

        public ActionResult Index(string moduleName, int? page)
        {
            var list = _moduleService.FindBy(moduleName, page.HasValue ? page.Value : 1, CustomDisplayExtensions.DefaultPageSize);

            ViewBag.Module = moduleName;

            return View(list);
        }

        public ActionResult EditModule(Guid? id)
        {
            var module = id.HasValue ? _moduleService.FindBy(id.Value) : new ModuleDTO();
            return View(module);
        }

        [HttpPost]
        public ActionResult SearchModule(string moduleName)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index", new
                    {
                        moduleName,
                    })
                });
            });
        }

        [HttpPost]
        public ActionResult EditModule(ModuleDTO module)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                if (module.Id == Guid.Empty)
                {
                    _moduleService.Add(module);
                    this.JsMessage = MessagesResources.Add_Success;
                }
                else
                {
                    _moduleService.Update(module);
                    this.JsMessage = MessagesResources.Update_Success;
                }
                base.ClearCacheModules();

                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                });
            });
        }

        public ActionResult RemoveModule(Guid id)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                _moduleService.Remove(id);
                base.ClearCacheModules();

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