using System.Net;
using System.Web.Mvc;
using log4net;
using Ruico.WebHost.Models;

namespace Ruico.WebHost
{
    //public class CustomAjaxExceptionAttribute : FilterAttribute, IExceptionFilter   //HandleErrorAttribute
    //{
    //    private static readonly ILog Log = LogManager.GetLogger(typeof(CustomAjaxExceptionAttribute));

    //    public void OnException(ExceptionContext filterContext)
    //    {
    //        if (filterContext.ExceptionHandled
    //            || !filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
    //        {
    //            return;
    //        }

    //        var ex = filterContext.Exception;

    //        filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;

    //        filterContext.Result = new JsonResult
    //        {
    //            Data = new AjaxResponse
    //            {
    //                Succeeded = false,
    //                ErrorMessage = ex.Message
    //            },
    //            JsonRequestBehavior = JsonRequestBehavior.AllowGet
    //        };

    //        //写入日志 记录
    //        Log.Error(ex.GetIndentedExceptionLog());

    //        filterContext.ExceptionHandled = true; //设置异常已经处理
    //        filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
    //    }
    //}
}
