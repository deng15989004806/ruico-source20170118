using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using Ruico.Dto.KaoQin;
using Ruico.Dto.UserSystem;
using Ruico.Infrastructure.Utility.Helper;

namespace Ruico.WebHost
{
    public static class CustomDisplayExtensions
    {
        public static string Display(this DateTime dateTime)
        {
            var t = dateTime.ToLocalTime();

            return t.ToString("yyyy/MM/dd HH:mm:ss");
        }

        public static string Display(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.Display() : "";
        }

        public static string DisplayDate(this DateTime dateTime)
        {
            var t = dateTime.ToLocalTime();

            return t.ToString("yyyy/MM/dd");
        }

        public static string DisplayDateWeekDay(this DateTime dateTime)
        {
            var t = dateTime.ToLocalTime();

            return t.ToString("yyyy/MM/dd ddd");
        }

        public static string DisplayDate(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.DisplayDate() : "";
        }

        public static string DisplayDateHourMinute(this DateTime dateTime)
        {
            var t = dateTime.ToLocalTime();

            return t.ToString("yyyy/MM/dd HH:mm");
        }

        public static string DisplayDateHourMinuteWeekDay(this DateTime dateTime)
        {
            var t = dateTime.ToLocalTime();

            return t.ToString("yyyy/MM/dd HH:mm ddd");
        }

        public static string DisplayDateHourMinute(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.DisplayDateHourMinute() : "";
        }

        public static string Display(this Decimal value)
        {
            return value.ToString("#,##0.00");
        }

        public static string Display(this Decimal? value)
        {
            return value.HasValue ? value.Value.Display() : "";
        }

        public static string HtmlLine(this string value)
        {
            return value.Replace("\n", "<br />");
        }

        public static PagedList.Mvc.PagedListRenderOptions PagedListRenderOptions
        {
            get
            {
                return new PagedList.Mvc.PagedListRenderOptions
                {
                    LinkToFirstPageFormat = "首页",
                    LinkToNextPageFormat = "下页",
                    LinkToPreviousPageFormat = "上页",
                    LinkToLastPageFormat = "末页",
                    MaximumPageNumbersToDisplay = 5,
                    DisplayEllipsesWhenNotShowingAllPageNumbers = false,
                };
            }
        }

        public static int DefaultPageSize = 15;

        public static List<SelectListItem> GetItemList(this List<ModuleDTO> list,
            string includeOption,
            string defValue = "")
        {
            var result = list.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = x.Id.ToString().EqualsIgnoreCase(defValue)
            }).ToList();

            if (includeOption.NotNullOrBlank())
            {
                result.Insert(0, new SelectListItem() { Text = includeOption, Value = "" });
            }

            return result;
        }

        public static void GetSortedList(
            this List<Ruico.Dto.Hr.DepartmentDTO> list,
            ref List<Ruico.Dto.Hr.DepartmentDTO> result,
            int parentId, int depth)
        {
            if (list.Any(x => x.ParentId == parentId))
            {
                foreach (var p in list.Where(x => x.ParentId == parentId))
                {
                    p.Depth = depth;
                    result.Add(p);
                    GetSortedList(list, ref result, p.DepartmentId, depth + 1);
                }
            }
        }

        public static void GetSortedList(
            this List<Domain.Weixin.Model.Department> list,
            ref List<Domain.Weixin.Model.Department> result,
            int parentId, int depth)
        {
            if (list.Any(x => x.ParentId == parentId))
            {
                foreach (var p in list.Where(x => x.ParentId == parentId))
                {
                    p.Depth = depth;
                    result.Add(p);
                    GetSortedList(list, ref result, p.Id, depth + 1);
                }
            }
        }

        #region 单选框和复选框的扩展
        /// <summary>
        /// 复选框,selValue为选中项
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="selectList"></param>
        /// <param name="selValue"></param>
        /// <returns></returns>
        public static MvcHtmlString CheckBox(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, IEnumerable<string> selValue)
        {
            return CheckBoxAndRadioFor<object, string>(name, selectList, false, selValue);
        }
        public static MvcHtmlString CheckBox(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, string selValue)
        {
            return CheckBox(htmlHelper, name, selectList, new List<string> { selValue });

        }
        /// <summary>
        /// 复选框
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="selectList"></param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxFor(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList)
        {
            return CheckBox(htmlHelper, name, selectList, new List<string>());
        }
        /// <summary>
        /// 根据列表输出checkbox
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="selectList"></param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList)
        {
            return CheckBoxFor(htmlHelper, expression, selectList, null);
        }

        /// <summary>
        ///  根据列表输出checkbox,selValue为默认选中的项
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="selectList"></param>
        /// <param name="selValue"></param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string selValue)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            return CheckBoxAndRadioFor<TModel, TProperty>(name, selectList, false, new List<string> { selValue });
        }
        /// <summary>
        /// 输出单选框和复选框
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="selectList"></param>
        /// <param name="isRadio"></param>
        /// <param name="selValue"></param>
        /// <returns></returns>
        static MvcHtmlString CheckBoxAndRadioFor<TModel, TProperty>(
            string name,
            IEnumerable<SelectListItem> selectList,
            bool isRadio,
            IEnumerable<string> selValue)
        {
            StringBuilder str = new StringBuilder();
            int c = 0;
            string check, activeClass;
            string type = isRadio ? "Radio" : "checkbox";

            foreach (var item in selectList)
            {
                c++;
                if (selValue != null && selValue.Contains(item.Value))
                {
                    check = "checked='checked'";
                    //activeClass = "style=color:red";
                    activeClass = string.Empty;
                }
                else
                {
                    check = string.Empty;
                    activeClass = string.Empty;
                }
                str.AppendFormat("<span><input type='{3}' value='{0}' name={1} id={1}{2} " + check + "/>", item.Value, name, c, type);
                str.AppendFormat("<label for='{0}{1}' {3}>{2}</lable></span>", name, c, item.Text, activeClass);

            }
            return MvcHtmlString.Create(str.ToString());
        }


        public static MvcHtmlString RadioButton(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, IEnumerable<string> selValue)
        {
            return CheckBoxAndRadioFor<object, string>(name, selectList, true, selValue);
        }
        /// <summary>
        /// 单选按钮组，seletList为选中项
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="selectList"></param>
        /// <param name="selValue"></param>
        /// <returns></returns>
        public static MvcHtmlString RadioButton(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList, string selValue)
        {
            return RadioButton(htmlHelper, name, selectList, new List<string> { selValue });
        }
        /// <summary>
        /// 单选按钮组
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="selectList"></param>
        /// <returns></returns>
        public static MvcHtmlString RadioButton(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList)
        {
            return RadioButton(htmlHelper, name, selectList, new List<string>());
        }
        /// <summary>
        ///  根据列表输出radiobutton
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="selectList"></param>
        /// <returns></returns>
        public static MvcHtmlString RadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList)
        {
            return RadioButtonFor(htmlHelper, expression, selectList, new List<string>());
        }
        public static MvcHtmlString RadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, IEnumerable<string> selValue)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            return CheckBoxAndRadioFor<TModel, TProperty>(name, selectList, true, selValue);
        }
        /// <summary>
        /// 根据列表输出radiobutton,selValue为默认选中的项
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="selectList"></param>
        /// <param name="selValue"></param>
        /// <returns></returns>
        public static MvcHtmlString RadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList, string selValue)
        {
            return RadioButtonFor(htmlHelper, expression, selectList, new List<string> { selValue });
        }
        #endregion

        #region Weixin
        public static string DisplayStatusText(this KaoQinStatusDTO staus)
        {
            switch (staus)
            {
                case KaoQinStatusDTO.Submited:
                    return "<span class=\"label label-info\">已提交</span>";
                case KaoQinStatusDTO.Canceled:
                    return "<span class=\"label label-inverse\">已取消</span>";
                case KaoQinStatusDTO.Approved:
                    return "<span class=\"label label-success\">已审核</span>";
                default:
                    return staus.ToString();
            }
        }

        public static List<SelectListItem> GetKaoQinStatusItemList(string includeOption,
            string defValue = "")
        {
            var result = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Text = "已提交",
                    Value = KaoQinStatusDTO.Submited.ToString(),
                    Selected = KaoQinStatusDTO.Submited.ToString().EqualsIgnoreCase(defValue)
                },
                new SelectListItem()
                {
                    Text = "已审核",
                    Value = KaoQinStatusDTO.Approved.ToString(),
                    Selected = KaoQinStatusDTO.Approved.ToString().EqualsIgnoreCase(defValue)
                },
                new SelectListItem()
                {
                    Text = "已取消",
                    Value = KaoQinStatusDTO.Canceled.ToString(),
                    Selected = KaoQinStatusDTO.Canceled.ToString().EqualsIgnoreCase(defValue)
                }
            };

            if (includeOption.NotNullOrBlank())
            {
                result.Insert(0, new SelectListItem() { Text = includeOption, Value = "" });
            }

            return result;
        }

        #endregion
    }
}