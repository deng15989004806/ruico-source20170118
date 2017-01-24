using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ruico.Application.Resources.Generated;
using Ruico.Dto.Weixin;

namespace Ruico.Application.Extensions
{
    public static class WeixinOperationLogExtension
    {
        #region 菜单

        public static string GetOperationLog(this AppMenuDTO entity, AppMenuDTO oldDTO = null)
        {
            var sb = new StringBuilder();

            if (oldDTO == null)
            {
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.Name, entity.Name));
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.Url, entity.Url));
                sb.AppendLine(string.Format("{0}: {1}", WeixinMessagesResources.Key, entity.Key));
                sb.AppendLine(string.Format("{0}: {1}", WeixinMessagesResources.AppId, entity.AppId));
            }
            else
            {
                if (entity.Name != oldDTO.Name)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.Name, oldDTO.Name, entity.Name));
                }
                if (entity.Url != oldDTO.Url)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.Url, oldDTO.Url, entity.Url));
                }
                if (entity.Key != oldDTO.Key)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        WeixinMessagesResources.Key, oldDTO.Key, entity.Key));
                }
                if (entity.AppId != oldDTO.AppId)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        WeixinMessagesResources.AppId, oldDTO.AppId, entity.AppId));
                }

                var parentName = string.Empty;
                if (entity.Parent != null)
                {
                    parentName = entity.Parent.Name;
                }

                var oldParentName = string.Empty;
                if (oldDTO.Parent != null)
                {
                    oldParentName = oldDTO.Parent.Name;
                }

                if (parentName != oldParentName)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        WeixinMessagesResources.Parent, oldParentName, parentName));
                }
            }

            return sb.ToString().TrimEnd('\r', '\n');
        }

        #endregion
    }
}
