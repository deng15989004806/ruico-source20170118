using System.Web.Mvc;
using log4net;
using Ruico.Application.SystemModule;
using Ruico.Dto.Base;

namespace Ruico.Application.BaseModule.Imp
{
    public class SerialNumberRuleQuerier
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SerialNumberRuleQuerier));

        protected static IServiceResolver ServiceResolver
        {
            get { return (IServiceResolver)DependencyResolver.Current.GetService(typeof(IServiceResolver)); }
        }

        protected static ISerialNumberRuleService SerialNumberRuleService
        {
            get { return ServiceResolver.Resolve<ISerialNumberRuleService>(); }
        }

        public static SerialNumberRuleDTO FindBy(string name)
        {
            return SerialNumberRuleService.FindBy(name);
        }
    }
}
