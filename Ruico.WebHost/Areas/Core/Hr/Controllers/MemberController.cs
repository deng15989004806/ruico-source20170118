using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ruico.Application.HrModule;
using Ruico.Domain.Weixin.Service;
using Ruico.Dto.Hr;
using Ruico.Infrastructure.Utility.Extensions;
using Ruico.WebHost.Models;
using Ruico.WebHost.Resources.Generated;

namespace Ruico.WebHost.Areas.Core.Hr.Controllers
{
    public class MemberController : CoreBaseController
    {
        IDepartmentService _departmentService;
        IMemberService _memberService;
        IContactsService _contactsService;
        ICommonService _commonService;

        #region Constructor

        public MemberController(IDepartmentService departmentService, 
            IMemberService memberService,
            IContactsService contactsService,
            ICommonService commonService)
        {
            _departmentService = departmentService;
            _memberService = memberService;
            _contactsService = contactsService;
            _commonService = commonService;
        }

        #endregion

        /// <summary>
        /// /Core/Weixin/Member/Index
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Index(string name, int? page)
        {
            var list = _memberService.FindBy(name, page.HasValue ? page.Value : 1, CustomDisplayExtensions.DefaultPageSize);
            
            ViewBag.Name = name;

            foreach (var member in list)
            {
                if (member.Status == 0)
                {
                    member.Status = 4;
                }
            }

            return View(list);
        }

        [HttpPost]
        public ActionResult SearchMember(string name)
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

        public ActionResult EditMember(Guid? id)
        {
            var member = id.HasValue
                ? _memberService.FindBy(id.Value)
                : new MemberDTO();

            if (member.Status == 0)
            {
                member.Status = 4;
            }

            return View(member);
        }

        [HttpPost]
        public ActionResult EditMember(MemberDTO member)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                member.Departments = new List<DepartmentDTO>();
                if (member.Id == Guid.Empty)
                {
                    _memberService.Add(member);
                    this.JsMessage = MessagesResources.Add_Success;
                }
                else
                {
                    _memberService.Update(member);
                    this.JsMessage = MessagesResources.Update_Success;
                }

                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                });
            });
        }

        public ActionResult RemoveMember(Guid id)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                _memberService.Remove(id);

                this.JsMessage = MessagesResources.Remove_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                }, JsonRequestBehavior.AllowGet);
            });
        }

        public ActionResult EditDepartmentList(Guid? memberId)
        {
            var member = memberId.HasValue
                ? _memberService.FindBy(memberId.Value)
                : new MemberDTO();

            if (member.Status == 0)
            {
                member.Status = 4;
            }

            var allDepartments = _departmentService.FindBy(null, null, 1, int.MaxValue);

            ViewBag.AllDepartments = allDepartments;

            return View(member);
        }

        [HttpPost]
        public ActionResult EditDepartmentList(Guid memberId, List<string> departments)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                var pList = new List<Guid>();

                foreach (var s in departments.OpSafe())
                {
                    Guid id;
                    if (Guid.TryParse(s, out id))
                    {
                        pList.Add(id);
                    }
                }

                _memberService.UpdateMemberDepartments(memberId, pList);

                this.JsMessage = MessagesResources.Update_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("EditDepartmentList", new
                    {
                        memberId
                    })
                });
            });
        }

        public ActionResult GetMembersFromWeixin()
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                var accessToken = _commonService.GetContactsAccessToken();
                var members = _contactsService.GetMembers(accessToken);
                var departments = _contactsService.GetDepartments(accessToken);

                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    Data = new
                    {
                        members = members.Select(x =>
                        {
                            var deps = x.Department.Select(id => departments.FirstOrDefault(d => d.Id == id));
                            return new
                            {
                                member = x,
                                departments = deps.Where(d => d != null)
                            };
                        })
                    }
                }, JsonRequestBehavior.AllowGet);
            });
        }

        public ActionResult DownloadMembersFromWeixin()
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                _memberService.DownloadMembers();

                this.JsMessage = MessagesResources.Download_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                }, JsonRequestBehavior.AllowGet);
            });
        }

        public ActionResult UploadMembersToWeixin()
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                _memberService.UploadMembers();

                this.JsMessage = MessagesResources.Upload_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                }, JsonRequestBehavior.AllowGet);
            });
        }

        public ActionResult RemoveNotExistMemberInWeixin()
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                _memberService.RemoveNotExistMemberInWeixin();

                this.JsMessage = MessagesResources.Remove_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("Index")
                }, JsonRequestBehavior.AllowGet);
            });
        }

        public ActionResult InviteMember()
        {
            var allDepartments = _departmentService.FindBy(null, null, 1, int.MaxValue);
            var allMembers = _memberService.FindBy(null, 1, int.MaxValue);

            ViewBag.AllDepartments = allDepartments.ToList();
            ViewBag.AllMembers = allMembers.ToList();

            return View();
        }

        [HttpPost]
        public ActionResult InviteMember(List<string> userIds)
        {
            return HttpHandleExtensions.AjaxCallGetResult(() =>
            {
                // 过滤一些值为false的hidden项
                userIds = userIds.Where(x => !string.IsNullOrWhiteSpace(x) && x != "false").ToList();

                _memberService.InviteMember(userIds);

                this.JsMessage = MessagesResources.Invite_Success;
                return Json(new AjaxResponse
                {
                    Succeeded = true,
                    RedirectUrl = Url.Action("InviteMember")
                });
            });
        }
    }
}