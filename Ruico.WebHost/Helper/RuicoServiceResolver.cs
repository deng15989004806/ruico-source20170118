using System;
using System.Web.Mvc;

namespace Ruico.WebHost.Helper
{
    public class RuicoServiceResolver
    {
        public static T Resolve<T>()
        {
            return (T)DependencyResolver.Current.GetService(typeof(T));
            //return ServiceLocator.Current.GetInstance<T>();
        }

        public static object Resolve(Type type)
        {
            var obj = DependencyResolver.Current.GetService(type);
            return obj;
            //var obj = ServiceLocator.Current.GetInstance(type);
            //return obj;
        }
    }
}