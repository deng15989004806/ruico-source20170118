using System.Web.Mvc;
using System.Web.Routing;
using log4net;
using Ruico.Application.SystemModule;
using Ruico.WebHost.Helper;

namespace Ruico.WebHost.Controllers
{
    public class BaseController : Controller
    {
        protected static readonly ILog Log = LogManager.GetLogger(typeof(BaseController));

        private const string JsMessageKey = "Ruico_JsMessageKey";

        IOperateLogService OperateLogService;

        public BaseController()
        {
            OperateLogService = RuicoServiceResolver.Resolve<IOperateLogService>();
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            ViewBag.JsMessage = this.JsMessage;
            // 清空消息
            this.JsMessage = string.Empty;
        }

        protected void RecordOperation(string sn, string operation, string message)
        {
            this.OperateLogService.RecordOperation(sn, operation, message);
        }

        /// <summary>
        /// 用于在页面载入时用js显示的消息
        /// </summary>
        protected string JsMessage
        {
            get
            {
                return Session[JsMessageKey] == null ? string.Empty
                    : Session[JsMessageKey] as string;
            }
            set
            {
                Session[JsMessageKey] = value;
            }
        }
    }
}