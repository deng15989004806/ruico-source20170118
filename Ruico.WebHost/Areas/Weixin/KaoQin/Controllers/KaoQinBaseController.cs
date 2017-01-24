using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Ruico.Application.Exceptions;
using Ruico.Application.HrModule;
using Ruico.Domain.Model;
using Ruico.Dto.KaoQin;

namespace Ruico.WebHost.Areas.Weixin.KaoQin.Controllers
{
    public class KaoQinBaseController : WeixinBaseController
    {
        protected KaoQinBaseController(IMemberService memberService)
            : base(memberService)
        {
        }

        protected DateTime GetConditionCreatedStartTime()
        {
            return DateTime.UtcNow.AddMonths(-1);
        }

        protected KaoQinConditionDTO GetApproveKaoQinCondition(
            string currentUserId,
            string currentUserPosition,
            List<string> status)
        {
            var excludePositions = new List<string>();
            if(currentUserPosition == MemberPositions.DepartmentSupervisor)
            {
                excludePositions.Add(MemberPositions.DepartmentSupervisor);
                excludePositions.Add(MemberPositions.DepartmentManager);
                excludePositions.Add(MemberPositions.CompanyLeader);
            }
            else if(currentUserPosition == MemberPositions.DepartmentManager)
            {
                excludePositions.Add(MemberPositions.DepartmentManager);
                excludePositions.Add(MemberPositions.CompanyLeader);
            }
            else if (currentUserPosition == MemberPositions.CompanyLeader)
            {
                excludePositions.Add(MemberPositions.CompanyLeader);
            }
            else
            {
                throw new DefinedException("UnKnowed Position: " + currentUserPosition);
            }
            var departmentIds = new List<int>();
            if (currentUserPosition != MemberPositions.CompanyLeader)
            {
                var dep = GetCurrentMemberDepartment();
                departmentIds.Add(dep.DepartmentId > 0 ? dep.DepartmentId : -1);
            }
            return new KaoQinConditionDTO()
            {
                Statuses = status,
                CreatedStartTime = GetConditionCreatedStartTime(),
                ExcludeUserId = currentUserId,
                ExcludePositions = excludePositions,
                DepartmentIds = departmentIds
            };
        }

        protected override string AppConfigName
        {
            get { return "KaoQin"; }
        }

        protected override string OAuth2Url
        {
            get { return "/weixin/kaoqin/oauth2"; }
        }

    }
}