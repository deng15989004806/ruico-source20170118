using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ruico.Application.HrModule;
using Ruico.Domain.Weixin.Service;
using Ruico.Dto.Hr;
using Ruico.WebHost.Models;
using Ruico.WebHost.Resources.Generated;

namespace Ruico.WebHost.Areas.Core.Hr.Controllers
{
    public class DepartmentController : CoreBaseController
    {
        IDepartmentService _departmentService;
        IContactsService _contactsService;
        ICommonService _commonService;

        #region Constructor

        public DepartmentController(IDepartmentService departmentService,
            IContactsService contactsService,
            ICommonService commonService)
        {
            _departmentService = departmentService;
            _contactsService = contactsService;
            _commonService = commonService;
        }

        #endregion

        /// <summary>
        /// /Core/Weixin/Department/Index
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(string name, int? page)
        {
            var list = _departmentService.FindBy(null, name, page.HasValue ? page.Value : 1, int.MaxValue);
            
            ViewBag.Name = name;

            return View(list);
        }

        [HttpPost]
        public ActionResult SearchDepartment(string name, int? parentId)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index", new
                    {
                        name = name,
                        parentId = parentId,
                    })
                });
            });
        }

        public ActionResult EditDepartment(Guid? id)
        {
            var department = id.HasValue
                ? _departmentService.FindBy(id.Value)
                : new DepartmentDTO();

            return View(department);
        }

        [HttpPost]
        public ActionResult EditDepartment(DepartmentDTO department)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                if (department.Id == Guid.Empty)
                {
                    _departmentService.Add(department);
                    this.JsMessage = MessagesResources.Add_Success;
                }
                else
                {
                    _departmentService.Update(department);
                    this.JsMessage = MessagesResources.Update_Success;
                }

                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                });
            });
        }

        public ActionResult RemoveDepartment(Guid id)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                _departmentService.Remove(id);

                this.JsMessage = MessagesResources.Remove_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                }, JsonRequestBehavior.AllowGet);
            });
        }

        public ActionResult GetDepartmentsFromWeixin()
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                var accessToken = _commonService.GetContactsAccessToken();
                var departments = _contactsService.GetDepartments(accessToken);


                var list = new List<Domain.Weixin.Model.Department>();
                departments.GetSortedList(ref list, 1, 1);

                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    Data = new {departments = list}
                }, JsonRequestBehavior.AllowGet);
            });
        }

        public ActionResult DownloadDepartmentsFromWeixin()
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                _departmentService.DownloadDepartments();

                this.JsMessage = MessagesResources.Download_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                }, JsonRequestBehavior.AllowGet);
            });
        }

        public ActionResult UploadDepartmentsToWeixin()
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                _departmentService.UploadDepartments();

                this.JsMessage = MessagesResources.Upload_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                }, JsonRequestBehavior.AllowGet);
            });
        }

        public ActionResult RemoveNotExistDepartmentInWeixin()
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                _departmentService.RemoveNotExistDepartmentInWeixin();

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