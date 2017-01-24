using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ruico.Domain.Model;
using Ruico.Domain.Weixin.Service;
using Senparc.Weixin.QY.AdvancedAPIs;
using Senparc.Weixin.QY.CommonAPIs;

namespace Ruico.Domain.Extend.Weixin
{
    public class CommonService : ICommonService
    {
        public string GetAccessToken(string appConfigName)
        {
            var corpId = WeixinConfig.GetCorpId();
            var secret = WeixinConfig.GetAppSecret(appConfigName);

            return AccessTokenContainer.TryGetToken(corpId, secret);
        }

        public string GetContactsAccessToken()
        {
            var corpId = WeixinConfig.GetCorpId();
            var secret = WeixinConfig.GetContactsSecret();

            return AccessTokenContainer.TryGetToken(corpId, secret);
        }

        public string GetAgentId(string appConfigName)
        {
            return WeixinConfig.GetAppId(appConfigName);
        }

        public string GetCode(string redirectUrl, string state)
        {
            var corpId = WeixinConfig.GetCorpId();
            return OAuth2Api.GetCode(corpId, redirectUrl, state);
        }

        public string GetUserId(string accessToken, string code)
        {
            return OAuth2Api.GetUserId(accessToken, code).UserId;
        }

        public IList<NameValue> GetApps()
        {
            var settings = WeixinConfig.GetAppsSetting();
            var result = new List<NameValue>();

            var items = settings.Split(';');
            foreach (var item in items)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    var app = item.Split(',');
                    if (app.Length >= 3 
                        && !string.IsNullOrWhiteSpace(app[0])
                        && !string.IsNullOrWhiteSpace(app[1])
                        && !string.IsNullOrWhiteSpace(app[2]))
                    {
                        result.Add(new NameValue() {Name = app[0], Value = app[1], Key = app[2]});
                    }
                }
            }

            return result;
        }
    }
}
