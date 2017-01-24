using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Ruico.IocModules
{
    public class RegisterModules
    {
        public static void RegisterSystemModules(ContainerBuilder builder)
        {
            builder.RegisterModule<CommonIocModule>();
            builder.RegisterModule<UserSystemIocModule>();
            builder.RegisterModule<SystemIocModule>();
            builder.RegisterModule<BaseIocModule>();
            builder.RegisterModule<WeixinIocModule>();
            builder.RegisterModule<HrIocModule>();
            builder.RegisterModule<KaoQinIocModule>();
        }
    }
}
