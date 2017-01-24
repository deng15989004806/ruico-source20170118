using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Transactions;
using log4net;
using Ruico.Infrastructure.Entity;
using Ruico.Infrastructure.Repository;
using Ruico.Infrastructure.UnitOfWork;

namespace Ruico.Repository
{
    public abstract class EfRepository<TEntity> : IRepository<TEntity>
        where TEntity : EntityBase
        //where TDbContext : System.Data.Entity.DbContext, new()
    {
        protected EfRepository(IUnitOfWork unitOfWork)
        {
            //this.DbContext = Activator.CreateInstance<TDbContext>();
            if (unitOfWork == (IUnitOfWork)null)
                throw new ArgumentNullException("unitOfWork");

            UnitOfWork = unitOfWork;
        }

        ///// <summary>
        ///// Gets or sets the DB context.
        ///// </summary>
        //protected TDbContext DbContext
        //{
        //    get;
        //    set;
        //}

        public IUnitOfWork UnitOfWork { get; private set; }

        private IDbSet<TEntity> _Table;

        protected IDbSet<TEntity> Table
        {
            get
            {
                if (_Table == null)
                {
                    _Table = (UnitOfWork as IRuicoUnitOfWork).CreateSet<TEntity>();
                }
                return _Table;
            }
        }

        public TEntity Get(object key)
        {
            return Table.Find(key);
        }

        public void Merge(TEntity persisted, TEntity current)
        {
            //if not is attached, attach original and set current values
            ((DbContext)UnitOfWork).Entry<TEntity>(persisted).CurrentValues.SetValues(current);
        }

        public IEnumerable<TEntity> FindAll()
        {
            return Table.ToList();
        }

        public void Add(TEntity item)
        {
            if (item == null) throw new ArgumentNullException("item");
            Table.Add(item);
        }

        public void Remove(TEntity item)
        {
            if (item == null) throw new ArgumentNullException("item");
            Table.Remove(item);
        }

        public TEntity Find(Func<TEntity, bool> acquire)
        {
            return Table.FirstOrDefault(acquire);
        }

        public IQueryable<TEntity> Collection
        {
            get { return Table.AsQueryable(); }
        }
    }
}
