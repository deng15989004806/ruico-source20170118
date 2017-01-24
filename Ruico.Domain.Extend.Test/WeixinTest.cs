using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ruico.Domain.Extend.Weixin;
using Ruico.Domain.Weixin.Model;
using Xunit;

namespace Ruico.Domain.Extend.Test
{
    public class WeixinTest
    {
        // 使用前先调用GetTokenTest，获取更新的AccessToken
        private const string AccessToken = "CnHwtbtHXyYl7Pq-ZBpW98oUauPAWoFPu7Pd6Q8nmiDvPiI5Elem21wl9OVnGUwCl2u1YkagnkOdtD-wCjCrqQ";

        [Fact]
        public void GetTokenTest()
        {
            var result = new CommonService().GetAccessToken("KaoQin");
            Console.WriteLine(result);
        }
        #region Department

        [Fact]
        public void GetDepartmentsTest()
        {
            var result = new ContactsService().GetDepartments(AccessToken);
            Console.WriteLine(JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void CreateDepartmentTest()
        {
            var result = new ContactsService().CreateDepartment(AccessToken, new Department(3, "人事部"));
            Console.WriteLine(JsonConvert.SerializeObject(result));
        }

        #endregion


        #region Member

        [Fact]
        public void GetMembersTest()
        {
            var result = new ContactsService().GetMembers(AccessToken);
            Console.WriteLine(JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void GetMemberInfo()
        {
            var result = new ContactsService().GetMemberInfo(AccessToken, "aqi");
            Console.WriteLine(JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void CreateMemberTest()
        {
            var result = new ContactsService().CreateMember(AccessToken, new Member("zhangshan", "张三", "zs9998")
            {
                Department = new List<int>() {3}
            });
            Console.WriteLine(JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void UpdateMemberTest()
        {
            var result = new ContactsService().UpdateMember(AccessToken, new Member("zhangshan", "张三", "zs9998")
            {
                Department = new List<int>() { 3 },
                Enable = 0
            });
            Console.WriteLine(JsonConvert.SerializeObject(result));
        }

        #endregion

        #region Menu

        [Fact]
        public void DeleteMenuTest()
        {
            var agentId = new CommonService().GetAgentId("KaoQin");
            var result = new MenuService().Delete(AccessToken, agentId);
            Console.WriteLine(JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void CreateMenuTest()
        {
            const string baseUrl = "http://183.63.45.178:81";
            var menus = new List<MenuGroup>()
            {
                new MenuGroup()
                {
                    Menu = new Menu("申请"),
                    Childs = new List<Menu>()
                    {
                        new Menu("view", "出差", null, baseUrl + "/WeiXin/KaoQin/ChuChai/Apply"),
                        new Menu("view", "外勤", null, baseUrl + "/WeiXin/KaoQin/WaiQin/Apply"),
                        new Menu("view", "请假", null, baseUrl + "/WeiXin/KaoQin/XiuJia/Apply"),
                        new Menu("view", "未打卡", null, baseUrl + "/WeiXin/KaoQin/WeiDaKa/Apply"),
                    }
                },
                new MenuGroup()
                {
                    Menu = new Menu("历史记录"),
                    Childs = new List<Menu>()
                    {
                        new Menu("view", "出差", null, baseUrl + "/WeiXin/KaoQin/ChuChai"),
                        new Menu("view", "外勤", null, baseUrl + "/WeiXin/KaoQin/WaiQin"),
                        new Menu("view", "请假", null, baseUrl + "/WeiXin/KaoQin/XiuJia"),
                        new Menu("view", "未打卡", null, baseUrl + "/WeiXin/KaoQin/WeiDaKa"),
                    }
                },
                new MenuGroup()
                {
                    Menu = new Menu("审批"),
                    Childs = new List<Menu>()
                    {
                        new Menu("view", "出差", null, baseUrl + "/WeiXin/KaoQin/ChuChai/PendingApprove"),
                        new Menu("view", "外勤", null, baseUrl + "/WeiXin/KaoQin/WaiQin/PendingApprove"),
                        new Menu("view", "请假", null, baseUrl + "/WeiXin/KaoQin/XiuJia/PendingApprove"),
                        new Menu("view", "未打卡", null, baseUrl + "/WeiXin/KaoQin/WeiDaKa/PendingApprove"),
                    }
                }
            };

            var agentId = new CommonService().GetAgentId("KaoQin");
            var result = new MenuService().Create(AccessToken, agentId, menus);
            Console.WriteLine(JsonConvert.SerializeObject(result));
        }

        [Fact]
        public void GetMenuTest()
        {
            var agentId = new CommonService().GetAgentId("KaoQin");
            var result = new MenuService().GetMenus(AccessToken, agentId);
            Console.WriteLine(JsonConvert.SerializeObject(result));
        }

        #endregion
    }
}
