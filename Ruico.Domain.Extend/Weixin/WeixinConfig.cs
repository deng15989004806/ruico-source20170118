using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruico.Domain.Extend.Weixin
{
    public class WeixinConfig
    {
        public static string GetCorpId()
        {
            const string settingName = "WeixinCorpID";
            var value = ConfigurationManager.AppSettings[settingName];
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ConfigurationErrorsException("missing \"" + settingName + "\" appSetting.");
            }
            return value;
        }

        public static string GetAppId(string appConfigName)
        {
            var settingName = string.Format("WeixinAppId:{0}", appConfigName);
            var value = ConfigurationManager.AppSettings[settingName];
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ConfigurationErrorsException("missing \"" + settingName + "\" appSetting.");
            }
            return value;
        }

        public static string GetAppSecret(string appConfigName)
        {
            var settingName = string.Format("WeixinSecret:{0}", appConfigName);
            var value = ConfigurationManager.AppSettings[settingName];
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ConfigurationErrorsException("missing \"" + settingName + "\" appSetting.");
            }
            return value;
        }

        public static string GetContactsSecret()
        {
            return GetAppSecret("Contacts");
        }

        /// <summary>
        /// 应用列表设置项目，格式：App名称1,AppId1;App名称2,AppId2
        /// </summary>
        /// <returns></returns>
        public static string GetAppsSetting()
        {
            var settingName = "WeixinApps";
            var value = ConfigurationManager.AppSettings[settingName];
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ConfigurationErrorsException("missing \"" + settingName + "\" appSetting.");
            }
            return value;
        }
    }
}
