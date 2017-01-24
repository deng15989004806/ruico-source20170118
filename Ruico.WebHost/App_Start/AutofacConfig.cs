using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Ruico.Application.SystemModule;
using Ruico.Infrastructure.Authorize;
using Ruico.IocModules;
using Ruico.WebHost.Helper;

namespace Ruico.WebHost
{
    public static class AutofacConfig
    {
        public static IContainer Container { get; private set; }

        public static void Config()
        {
            var builder = new ContainerBuilder();

            //builder.RegisterModule(new ConfigurationSettingsReader("autofac"));

            RegisterForMvc(builder);
            RegisterForWebApi(builder);
            RegisterSystemModules(builder);

            builder.RegisterType<AuthorizeManager>().As<IAuthorizeManager>().InstancePerRequest();
            builder.RegisterType<AuthorizeManager>().As<IAuthorizeManager>().InstancePerLifetimeScope();

            builder.RegisterType<ServiceResolver>().As<IServiceResolver>();

            //builder.RegisterType<BaseAuthorizeController>().PropertiesAutowired();

            Container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));//注册mvc容器
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(Container);
        }

        private static void RegisterForMvc(ContainerBuilder builder)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();

            builder.RegisterControllers(executingAssembly).InstancePerRequest();
            builder.RegisterModelBinders(executingAssembly);
            builder.RegisterModelBinderProvider();
            builder.RegisterModule(new AutofacWebTypesModule());
            builder.RegisterSource(new ViewRegistrationSource());
            builder.RegisterFilterProvider();
        }

        private static void RegisterForWebApi(ContainerBuilder builder)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();

            builder.RegisterApiControllers(executingAssembly).InstancePerRequest();
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);
        }

        private static void RegisterSystemModules(ContainerBuilder builder)
        {
            // 注册系统定义的模块
            RegisterModules.RegisterSystemModules(builder);
        }
    }
}