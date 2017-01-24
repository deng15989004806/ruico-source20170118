using Autofac;
using Ruico.Application.UserSystemModule;
using Ruico.Application.UserSystemModule.Imp;
using Ruico.Domain.UserSystemModule.Repositories;
using Ruico.Repository.UserSystemModule.Repositories;
using Module = Autofac.Module;

namespace Ruico.IocModules
{
    public class UserSystemIocModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ModuleRepository>().As<IModuleRepository>().InstancePerRequest();
            builder.RegisterType<ModuleService>().As<IModuleService>();

            builder.RegisterType<MenuRepository>().As<IMenuRepository>().InstancePerRequest();
            builder.RegisterType<PermissionRepository>().As<IPermissionRepository>().InstancePerRequest();
            builder.RegisterType<MenuService>().As<IMenuService>();

            builder.RegisterType<RoleRepository>().As<IRoleRepository>().InstancePerRequest();
            builder.RegisterType<RoleService>().As<IRoleService>();

            builder.RegisterType<RoleGroupRepository>().As<IRoleGroupRepository>().InstancePerRequest();
            builder.RegisterType<RoleGroupService>().As<IRoleGroupService>();

            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerRequest();
            builder.RegisterType<UserService>().As<IUserService>();

            builder.RegisterType<AuthService>().As<IAuthService>();
        }
    }
}
