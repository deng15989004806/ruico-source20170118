using Autofac;
using Ruico.Application.WeixinModule;
using Ruico.Application.WeixinModule.Imp;
using Ruico.Domain.Extend.Weixin;
using Ruico.Domain.Weixin.Service;
using Ruico.Domain.WeixinModule.Repositories;
using Ruico.Repository.WeixinModule.Repositories;
using Module = Autofac.Module;

namespace Ruico.IocModules
{
    public class WeixinIocModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CommonService>().As<ICommonService>();
            builder.RegisterType<ContactsService>().As<IContactsService>();
            builder.RegisterType<MenuService>().As<IMenuService>();
            
            builder.RegisterType<AppMenuRepository>().As<IAppMenuRepository>().InstancePerRequest();
            builder.RegisterType<AppMenuService>().As<IAppMenuService>();
        }
    }
}
