using Ruico.Application.SystemModule;

namespace Ruico.WebHost.Helper
{
    public class ServiceResolver : IServiceResolver
    {
        public T Resolve<T>()
        {
            return RuicoServiceResolver.Resolve<T>();
        }
    }
}