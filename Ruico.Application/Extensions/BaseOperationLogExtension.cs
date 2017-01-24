using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ruico.Application.Resources.Generated;
using Ruico.Dto.Base;
using Ruico.Dto.System;

namespace Ruico.Application.Extensions
{
    public static class BaseOperationLogExtension
    {
        #region 货品

        public static string GetOperationLog(this CargoDTO entity, CargoDTO oldDTO = null)
        {
            var sb = new StringBuilder();

            if (oldDTO == null)
            {
                var categoryName = string.Empty;
                if (entity.Category != null)
                {
                    categoryName = entity.Category.Name;
                }
                sb.AppendLine(string.Format("{0}: {1}", BaseMessagesResources.Category, categoryName));
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.Sn, entity.Sn));
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.Name, entity.Name));
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.Description, entity.Description));
            }
            else
            {
                if (entity.Name != oldDTO.Name)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.Name, oldDTO.Name, entity.Name));
                }
                if (entity.Description != oldDTO.Description)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.Description, oldDTO.Description, entity.Description));
                }

                var categoryName = string.Empty;
                if (entity.Category != null)
                {
                    categoryName = entity.Category.Name;
                }

                var oldCategoryName = string.Empty;
                if (oldDTO.Category != null)
                {
                    oldCategoryName = oldDTO.Category.Name;
                }

                if (categoryName != oldCategoryName)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        BaseMessagesResources.Category, oldCategoryName, categoryName));
                }
            }

            return sb.ToString().TrimEnd('\r', '\n');
        }

        #endregion

        #region

        public static string GetOperationLog(this CategoryDTO entity, CategoryDTO oldDTO = null)
        {
            var sb = new StringBuilder();

            var parentNames = new List<string>();
            var parent = entity.Parent;
            while (parent != null)
            {
                parentNames.Add(parent.Name);
                parent = parent.Parent;
            }
            parentNames.Reverse();
            var parentNameString = string.Join(" > ", parentNames.ToArray());

            if (oldDTO == null)
            {
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.Sn, entity.Sn));
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.Name, entity.Name));
                sb.AppendLine(string.Format("{0}: {1}", BaseMessagesResources.Parent_Category, parentNameString));
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.SortOrder, entity.SortOrder));
                sb.AppendLine(string.Format("{0}: {1}", BaseMessagesResources.Child_SerialNumberRule_Prefix, entity.ChildSnRulePrefix));
                sb.AppendLine(string.Format("{0}: {1}", BaseMessagesResources.Child_SerialNumberRule_NumberLength, entity.ChildSnRuleNumberLength));
            }
            else
            {
                var oldParentNames = new List<string>();
                var oldParent = oldDTO.Parent;
                while (oldParent != null)
                {
                    oldParentNames.Add(oldParent.Name);
                    oldParent = oldParent.Parent;
                }
                oldParentNames.Reverse();
                var oldParentNameString = string.Join(" > ", oldParentNames.ToArray());

                if (entity.Sn != oldDTO.Sn)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.Sn, oldDTO.Sn, entity.Sn));
                }
                if (entity.Name != oldDTO.Name)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.Name, oldDTO.Name, entity.Name));
                }
                if (parentNameString != oldParentNameString)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                       BaseMessagesResources.Parent_Category, parentNameString, oldParentNameString));
                }
                if (entity.SortOrder != oldDTO.SortOrder)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.SortOrder, oldDTO.SortOrder, entity.SortOrder));
                }
                if (entity.ChildSnRulePrefix != oldDTO.ChildSnRulePrefix)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        BaseMessagesResources.Child_SerialNumberRule_Prefix, oldDTO.ChildSnRulePrefix, entity.ChildSnRulePrefix));
                }
                if (entity.ChildSnRuleNumberLength != oldDTO.ChildSnRuleNumberLength)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        BaseMessagesResources.Child_SerialNumberRule_NumberLength, oldDTO.ChildSnRuleNumberLength, entity.ChildSnRuleNumberLength));
                }
            }

            return sb.ToString().TrimEnd('\r', '\n');
        }

        #endregion

        #region

        public static string GetOperationLog(this SerialNumberRuleDTO entity, SerialNumberRuleDTO oldDTO = null)
        {
            var sb = new StringBuilder();

            if (oldDTO == null)
            {
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.Name, entity.RuleName));
                sb.AppendLine(string.Format("{0}: {1}", BaseMessagesResources.SerialNumberRule_Prefix, entity.Prefix));
                sb.AppendLine(string.Format("{0}: {1}", BaseMessagesResources.SerialNumberRule_UseDateNumber, entity.UseDateNumber));
                sb.AppendLine(string.Format("{0}: {1}", BaseMessagesResources.SerialNumberRule_NumberLength, entity.NumberLength));
            }
            else
            {
                if (entity.RuleName != oldDTO.RuleName)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.Name, oldDTO.RuleName, entity.RuleName));
                }
                if (entity.Prefix != oldDTO.Prefix)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        BaseMessagesResources.SerialNumberRule_Prefix, oldDTO.Prefix, entity.Prefix));
                }
                if (entity.UseDateNumber != oldDTO.UseDateNumber)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                         BaseMessagesResources.SerialNumberRule_UseDateNumber, oldDTO.UseDateNumber, entity.UseDateNumber));
                }
                if (entity.NumberLength != oldDTO.NumberLength)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                       BaseMessagesResources.SerialNumberRule_NumberLength, oldDTO.NumberLength, entity.NumberLength));
                }
            }

            return sb.ToString().TrimEnd('\r', '\n');
        }

        #endregion

        #region 船舶

        public static string GetOperationLog(this ShipDTO entity, ShipDTO oldDTO = null)
        {
            var sb = new StringBuilder();

            if (oldDTO == null)
            {
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.Sn, entity.Sn));
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.Name, entity.Name));
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.Description, entity.Description));
            }
            else
            {
                if (entity.Name != oldDTO.Name)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.Name, oldDTO.Name, entity.Name));
                }
                if (entity.Description != oldDTO.Description)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.Description, oldDTO.Description, entity.Description));
                }
            }

            return sb.ToString().TrimEnd('\r', '\n');
        }

        #endregion

        #region 公司

        public static string GetOperationLog(this CompanyDTO entity, CompanyDTO oldDTO = null)
        {
            var sb = new StringBuilder();

            if (oldDTO == null)
            {
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.Sn, entity.Sn));
                sb.AppendLine(string.Format("{0}: {1}", CommonMessageResources.Name, entity.Name));
                sb.AppendLine(string.Format("{0}: {1}", BaseMessagesResources.Contact, entity.Contact));
                sb.AppendLine(string.Format("{0}: {1}", BaseMessagesResources.Phone, entity.Phone));
                sb.AppendLine(string.Format("{0}: {1}", BaseMessagesResources.Fax, entity.Fax));
                sb.AppendLine(string.Format("{0}: {1}", BaseMessagesResources.Mobile, entity.Mobile));
                sb.AppendLine(string.Format("{0}: {1}", BaseMessagesResources.Address, entity.Address));
                sb.AppendLine(string.Format("{0}: {1}", BaseMessagesResources.Postcode, entity.Postcode));
                sb.AppendLine(string.Format("{0}: {1}", BaseMessagesResources.Email, entity.Email));
            }
            else
            {
                if (entity.Name != oldDTO.Name)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        CommonMessageResources.Name, oldDTO.Name, entity.Name));
                }
                if (entity.Contact != oldDTO.Contact)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        BaseMessagesResources.Contact, oldDTO.Contact, entity.Contact));
                }
                if (entity.Phone != oldDTO.Phone)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        BaseMessagesResources.Phone, oldDTO.Phone, entity.Phone));
                }
                if (entity.Fax != oldDTO.Fax)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        BaseMessagesResources.Fax, oldDTO.Fax, entity.Fax));
                }
                if (entity.Mobile != oldDTO.Mobile)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        BaseMessagesResources.Mobile, oldDTO.Mobile, entity.Mobile));
                }
                if (entity.Address != oldDTO.Address)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        BaseMessagesResources.Address, oldDTO.Address, entity.Address));
                }
                if (entity.Postcode != oldDTO.Postcode)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        BaseMessagesResources.Postcode, oldDTO.Postcode, entity.Postcode));
                }
                if (entity.Email != oldDTO.Email)
                {
                    sb.AppendLine(string.Format("{0}: {1} => {2}",
                        BaseMessagesResources.Email, oldDTO.Email, entity.Email));
                }
            }

            return sb.ToString().TrimEnd('\r', '\n');
        }

        #endregion
    }
}
