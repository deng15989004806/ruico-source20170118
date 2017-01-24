using Autofac;
using Ruico.Infrastructure.Authorize;
using Ruico.Infrastructure.UnitOfWork;
using Ruico.Infrastructure.Utility.Caching;
using Ruico.Repository;
using Module = Autofac.Module;

namespace Ruico.IocModules
{
    public class CommonIocModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RuicoUnitOfWork>().As<IUnitOfWork>().InstancePerRequest();

            builder.RegisterType<LocalCacheManager>().As<ICacheManager>().InstancePerRequest();

            builder.RegisterType<AuthorizeFilter>().PropertiesAutowired();
        }
    }
}
