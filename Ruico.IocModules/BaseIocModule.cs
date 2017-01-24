using Autofac;
using Ruico.Application.BaseModule;
using Ruico.Application.BaseModule.Imp;
using Ruico.Domain.BaseModule.Repositories;
using Ruico.Repository.BaseModule.Repositories;
using Module = Autofac.Module;

namespace Ruico.IocModules
{
    public class BaseIocModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SerialNumberRuleRepository>().As<ISerialNumberRuleRepository>().InstancePerRequest();
            builder.RegisterType<SerialNumberRuleService>().As<ISerialNumberRuleService>();

            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>().InstancePerRequest();
            builder.RegisterType<CategoryService>().As<ICategoryService>();

            builder.RegisterType<CargoRepository>().As<ICargoRepository>().InstancePerRequest();
            builder.RegisterType<CargoService>().As<ICargoService>();

            builder.RegisterType<CompanyRepository>().As<ICompanyRepository>().InstancePerRequest();
            builder.RegisterType<CompanyService>().As<ICompanyService>();

            builder.RegisterType<ShipRepository>().As<IShipRepository>().InstancePerRequest();
            builder.RegisterType<ShipService>().As<IShipService>();
        }
    }
}
