using System;
using System.Web.Mvc;
using Ruico.Application.BaseModule;
using Ruico.Dto.Base;
using Ruico.WebHost.Models;
using Ruico.WebHost.Resources.Generated;

namespace Ruico.WebHost.Areas.Core.Base.Controllers
{
    public class CategoryController : CoreBaseController
    {
        ICategoryService _categoryService;
        ISerialNumberRuleService _serialNumberRuleService;

        #region Constructor

        public CategoryController(ICategoryService categoryService,
            ISerialNumberRuleService serialNumberRuleService)
        {
            _categoryService = categoryService;
            _serialNumberRuleService = serialNumberRuleService;
        }

        #endregion

        /// <summary>
        /// /Base/Category/Index
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(string name, int? page)
        {
            var list = _categoryService.FindBy(name, 1, null, page.HasValue ? page.Value : 1, CustomDisplayExtensions.DefaultPageSize);

            ViewBag.Name = name;

            return View(list);
        }

        [HttpPost]
        public ActionResult SearchCategory(string name)
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

        public ActionResult EditCategory(Guid? id)
        {
            var category = id.HasValue
                ? _categoryService.FindBy(id.Value)
                : new CategoryDTO();

            return View(category);
        }

        [HttpPost]
        public ActionResult EditCategory(CategoryDTO category)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                if (category.Id == Guid.Empty)
                {
                    category.CreatorId = GetCurrentUser().UserId;
                    _categoryService.Add(category);
                    this.JsMessage = MessagesResources.Add_Success;
                }
                else
                {
                    _categoryService.Update(category);
                    this.JsMessage = MessagesResources.Update_Success;
                }

                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                });
            });
        }

        public ActionResult RemoveCategory(Guid id)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                _categoryService.Remove(id);

                this.JsMessage = MessagesResources.Remove_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                }, JsonRequestBehavior.AllowGet);
            });
        }

        /// <summary>
        /// /Base/Category/ChildList
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentId"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult ChildList(string name, Guid parentId, int? page)
        {
            var list = _categoryService.FindBy(name, 0, parentId, page.HasValue ? page.Value : 1, CustomDisplayExtensions.DefaultPageSize);

            ViewBag.Name = name;
            ViewBag.Parent = _categoryService.FindBy(parentId);

            return View(list);
        }

        [HttpPost]
        public ActionResult SearchChildCategory(string name, Guid parentId)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("ChildList", new
                    {
                        name = name,
                        parentId = parentId,
                    })
                });
            });
        }


        public ActionResult EditChildCategory(Guid? id, Guid parentId)
        {
            var category = id.HasValue
                ? _categoryService.FindBy(id.Value)
                : new CategoryDTO();

            ViewBag.Parent = _categoryService.FindBy(parentId);

            return View(category);
        }

        [HttpPost]
        public ActionResult EditChildCategory(CategoryDTO category, Guid parentId)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                if (category.Id == Guid.Empty)
                {
                    category.CreatorId = GetCurrentUser().UserId;
                    category.Parent = new CategoryDTO() {Id = parentId};
                    _categoryService.Add(category);
                    this.JsMessage = MessagesResources.Add_Success;
                }
                else
                {
                    _categoryService.Update(category);
                    this.JsMessage = MessagesResources.Update_Success;
                }

                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("ChildList", new {parentId = parentId})
                });
            });
        }


        public ActionResult RemoveChildCategory(Guid id, Guid parentId)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                _categoryService.Remove(id);

                this.JsMessage = MessagesResources.Remove_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("ChildList", new {parentId = parentId})
                }, JsonRequestBehavior.AllowGet);
            });
        }
    }
}