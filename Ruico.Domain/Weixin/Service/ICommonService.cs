using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ruico.Domain.Model;
using Ruico.Domain.Weixin.Model;

namespace Ruico.Domain.Weixin.Service
{
    /// <summary>
    /// 企业号通用
    /// </summary>
    public interface ICommonService
    {
        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <param name="appConfigName">企业应用配置名，如KaoQin</param>
        /// <returns></returns>
        string GetAccessToken(string appConfigName);

        /// <summary>
        /// 获取访问部门和成员的AccessToken
        /// </summary>
        /// <returns></returns>
        string GetContactsAccessToken();

        /// <summary>
        /// 获取应用Id
        /// </summary>
        /// <param name="appConfigName">企业应用配置名，如KaoQin</param>
        /// <returns></returns>
        string GetAgentId(string appConfigName);

        /// <summary>
        /// 企业获取code
        /// </summary>
        /// <param name="redirectUrl">授权后重定向的回调链接地址，请使用urlencode对链接进行处理</param>
        /// <param name="state">重定向后会带上state参数，企业可以填写a-zA-Z0-9的参数值</param>
        /// <returns></returns>
        /// 员工点击后，页面将跳转至 redirect_uri/?code=CODE&state=STATE，企业可根据code参数获得员工的userid
        string GetCode(string redirectUrl, string state);
        
        /// <summary>
        /// 获取成员UserId
        /// </summary>
        /// <param name="accessToken">AccessToken</param>
        /// <param name="code">过员工授权获取到的code，每次员工授权带上的code将不一样，code只能使用一次，5分钟未被使用自动过期</param>
        /// <returns></returns>
        string GetUserId(string accessToken, string code);

        /// <summary>
        /// 获取应用列表(名称+AppId+AppConfigName)
        /// </summary>
        /// <returns></returns>
        IList<NameValue> GetApps();
    }
}
