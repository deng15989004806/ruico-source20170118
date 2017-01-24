using Autofac;
using Ruico.Application.KaoQinModule;
using Ruico.Application.KaoQinModule.Imp;
using Ruico.Domain.KaoQinModule.Repositories;
using Ruico.Repository.KaoQinModule.Repositories;
using Module = Autofac.Module;

namespace Ruico.IocModules
{
    public class KaoQinIocModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ChuChaiRepository>().As<IChuChaiRepository>().InstancePerRequest();
            builder.RegisterType<ChuChaiService>().As<IChuChaiService>();

            builder.RegisterType<WaiQinRepository>().As<IWaiQinRepository>().InstancePerRequest();
            builder.RegisterType<WaiQinService>().As<IWaiQinService>();

            builder.RegisterType<WeiDaKaRepository>().As<IWeiDaKaRepository>().InstancePerRequest();
            builder.RegisterType<WeiDaKaService>().As<IWeiDaKaService>();

            builder.RegisterType<XiuJiaRepository>().As<IXiuJiaRepository>().InstancePerRequest();
            builder.RegisterType<XiuJiaService>().As<IXiuJiaService>();
        }
    }
}
