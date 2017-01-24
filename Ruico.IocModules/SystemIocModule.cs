using Autofac;
using Ruico.Application.SystemModule;
using Ruico.Application.SystemModule.Imp;
using Ruico.Application.UserSystemModule;
using Ruico.Domain.SystemModule.Repositories;
using Ruico.Domain.UserSystemModule.Repositories;
using Ruico.Repository.SystemModule.Repositories;
using Ruico.Repository.UserSystemModule.Repositories;
using Module = Autofac.Module;

namespace Ruico.IocModules
{
    public class SystemIocModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OperateRecordRepository>().As<IOperateRecordRepository>().InstancePerRequest();
            builder.RegisterType<OperateRecordService>().As<IOperateRecordService>();
            builder.RegisterType<OperateLogService>().As<IOperateLogService>();

            builder.RegisterType<SerialNumberRepository>().As<ISerialNumberRepository>().InstancePerRequest();
            builder.RegisterType<SerialNumberService>().As<ISerialNumberService>();
        }
    }
}
