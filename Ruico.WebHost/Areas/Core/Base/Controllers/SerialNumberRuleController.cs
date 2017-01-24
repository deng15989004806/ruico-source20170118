using System;
using System.Web.Mvc;
using Ruico.Application.BaseModule;
using Ruico.Dto.Base;
using Ruico.WebHost.Models;
using Ruico.WebHost.Resources.Generated;

namespace Ruico.WebHost.Areas.Core.Base.Controllers
{
    public class SerialNumberRuleController : CoreBaseController
    {
        ISerialNumberRuleService _serialNumberRuleService;

        #region Constructor

        public SerialNumberRuleController(ISerialNumberRuleService serialNumberRuleService)
        {
            _serialNumberRuleService = serialNumberRuleService;
        }

        #endregion

        /// <summary>
        /// /Base/SerialNumberRule/Index
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(string name, int? page)
        {
            var list = _serialNumberRuleService.FindBy(name, page.HasValue ? page.Value : 1, CustomDisplayExtensions.DefaultPageSize);

            ViewBag.Name = name;

            return View(list);
        }

        public ActionResult EditSerialNumberRule(Guid? id)
        {
            var serialNumberRule = id.HasValue
                ? _serialNumberRuleService.FindBy(id.Value)
                : new SerialNumberRuleDTO() {NumberLength = 3};
            return View(serialNumberRule);
        }

        [HttpPost]
        public ActionResult SearchSerialNumberRule(string name)
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
        public ActionResult EditSerialNumberRule(SerialNumberRuleDTO serialNumberRule)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                if (serialNumberRule.Id == Guid.Empty)
                {
                    serialNumberRule.CreatorId = GetCurrentUser().UserId;
                    _serialNumberRuleService.Add(serialNumberRule);
                    this.JsMessage = MessagesResources.Add_Success;
                }
                else
                {
                    _serialNumberRuleService.Update(serialNumberRule);
                    this.JsMessage = MessagesResources.Update_Success;
                }

                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                });
            });
        }

        public ActionResult RemoveSerialNumberRule(Guid id)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                _serialNumberRuleService.Remove(id);

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