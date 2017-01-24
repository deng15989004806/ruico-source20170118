using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using log4net;
using Ruico.Application.Exceptions;
using Ruico.WebHost.Models;

namespace Ruico.WebHost
{
    public static class HttpHandleExtensions
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HttpHandleExtensions));

        public static ActionResult AjaxCallGetResult(Func<ActionResult> call)
        {
            ActionResult result;
            try
            {
                result = call();
            }
            catch (Exception ex)
            {
                if (!(ex is DefinedException))
                {
                    //写入日志 记录
                    Log.Error(ex.GetIndentedExceptionLog());
                }

                result = new JsonResult
                {
                    Data = new AjaxResponse
                    {
                        Succeeded = false,
                        ErrorMessage = ex.Message
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            return result;
        }
        
    }
}