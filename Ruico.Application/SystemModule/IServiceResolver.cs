namespace Ruico.Application.SystemModule
{
    public interface IServiceResolver
    {
        T Resolve<T>();
    }
}
