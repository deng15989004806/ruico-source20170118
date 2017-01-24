using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ruico.Domain.Weixin.Model;

namespace Ruico.Domain.Weixin.Service
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    public interface IMenuService
    {
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="accessToken">AccessToken</param>
        /// <param name="agentId">企业应用的id，整型。可在应用的设置页面查看</param>
        QyCallResult Delete(string accessToken, string agentId);

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="accessToken">AccessToken</param>
        /// <param name="agentId">企业应用的id，整型。可在应用的设置页面查看</param>
        /// <param name="menus"></param>
        QyCallResult Create(string accessToken, string agentId, List<MenuGroup> menus);

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="agentId"></param>
        /// <returns></returns>
        List<MenuGroup> GetMenus(string accessToken, string agentId);
    }
}
