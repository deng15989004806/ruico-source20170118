using System.Data.Entity;
using Ruico.Infrastructure.UnitOfWork;

namespace Ruico.Repository
{
    public interface IRuicoUnitOfWork : IUnitOfWork
    {
        IDbSet<TEntity> CreateSet<TEntity>() where TEntity : class;

        DbContext DbContext { get; }
    }
}
