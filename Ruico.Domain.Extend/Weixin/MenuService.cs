using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Ruico.Domain.Weixin.Model;
using Ruico.Domain.Weixin.Service;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.HttpUtility;
using Senparc.Weixin.QY.CommonAPIs;

namespace Ruico.Domain.Extend.Weixin
{
    public class MenuService : IMenuService
    {
        public QyCallResult Delete(string accessToken, string agentId)
        {
            var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/menu/delete?access_token={0}&agentid={1}", accessToken, agentId);
            return Get.GetJson<QyJsonResult>(url).ToQyCallResult();
        }

        public QyCallResult Create(string accessToken, string agentId, List<MenuGroup> menus)
        {
            var urlFormat = string.Format("https://qyapi.weixin.qq.com/cgi-bin/menu/create?access_token={0}&agentid={1}", accessToken, agentId);

            var data = new
            {
                button = menus.Select(x => new
                {
                    type = x.Menu.Type,
                    name = x.Menu.Name,
                    key = x.Menu.Key,
                    url = x.Menu.Url,
                    sub_button = x.Childs.Select(c=>new
                    {
                        type = c.Type,
                        name = c.Name,
                        key = c.Key,
                        url = c.Url,
                    })
                }).ToArray()
            };

            return CommonJsonSend.Send(null, urlFormat, data, CommonJsonSendType.POST).ToQyCallResult();
        }

        public List<MenuGroup> GetMenus(string accessToken, string agentId)
        {
            var url = string.Format("https://qyapi.weixin.qq.com/cgi-bin/menu/get?access_token={0}&agentid={1}", accessToken, agentId);
            var result = Get.GetJson<MenuButtonResult>(url);

            return result.menu.button.Select(x=>new MenuGroup()
            {
                Menu =new Menu(x.name),
                Childs = x.sub_button.Select(s=>new Menu(s.type,s.name,s.key,s.url)).ToList()
            }).ToList();
        }
    }

    public class MenuButtonResult
    {
        public MenuButtonMenu menu { get; set; }
    }

    public class MenuButtonMenu
    {
        public List<MenuButton> button { get; set; }
    }

    public class MenuButton
    {
        public string name { get; set; }

        public List<MenuSubButton> sub_button { get; set; }
    }

    public class MenuSubButton
    {
        public string url { get; set; }
        public string key { get; set; }
        public string type { get; set; }
        public string name { get; set; }
    }
}
