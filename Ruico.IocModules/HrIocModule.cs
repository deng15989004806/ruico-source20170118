using Autofac;
using Ruico.Application.HrModule;
using Ruico.Application.HrModule.Imp;
using Ruico.Application.WeixinModule;
using Ruico.Domain.Extend.Weixin;
using Ruico.Domain.HrModule.Repositories;
using Ruico.Domain.Weixin.Service;
using Ruico.Domain.WeixinModule.Repositories;
using Ruico.Repository.HrModule.Repositories;
using Ruico.Repository.WeixinModule.Repositories;
using Module = Autofac.Module;

namespace Ruico.IocModules
{
    public class HrIocModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DepartmentRepository>().As<IDepartmentRepository>().InstancePerRequest();
            builder.RegisterType<DepartmentService>().As<IDepartmentService>();

            builder.RegisterType<MemberRepository>().As<IMemberRepository>().InstancePerRequest();
            builder.RegisterType<MemberService>().As<IMemberService>();
        }
    }
}
