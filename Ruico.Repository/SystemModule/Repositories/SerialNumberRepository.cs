using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.Extensions;
using PagedList;
using Ruico.Domain.SystemModule.Entities;
using Ruico.Domain.SystemModule.Repositories;
using Ruico.Infrastructure.UnitOfWork;

namespace Ruico.Repository.SystemModule.Repositories
{
    public class SerialNumberRepository : EfRepository<SerialNumber>, ISerialNumberRepository
    {
        public SerialNumberRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            if (unitOfWork == (IUnitOfWork)null)
                throw new ArgumentNullException("unitOfWork");

            UnitOfWork = unitOfWork;
        }

        protected IUnitOfWork UnitOfWork { get; private set; }

        public int GetSerialNumber(string prefix, string dateNumber, int increase)
        {
            var unitOfWork = (this.UnitOfWork as RuicoUnitOfWork);
            if (unitOfWork == null)
                throw new ArgumentNullException("unitOfWork");

            var parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@Prefix", prefix);
            parameters[1] = new SqlParameter("@DateNumber", dateNumber);
            parameters[2] = new SqlParameter("@Increase", increase);
            return unitOfWork.Database.SqlQuery<int>("exec [system].[GetSerialNumber] @Prefix,@DateNumber,@Increase", parameters).FirstOrDefault();
        }

        public IPagedList<SerialNumber> FindBy(int pageNumber, int pageSize)
        {
            IQueryable<SerialNumber> entities = Table;

            var totalCountQuery = entities.FutureCount();
            var resultQuery = entities
                .OrderBy(x => x.Created)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Future();

            var totalCount = totalCountQuery.Value;
            var result = resultQuery.ToList();

            return new StaticPagedList<SerialNumber>(
                result,
                pageNumber,
                pageSize,
                totalCount);
        }

        public void Remove(long id)
        {
            this.Remove(this.Get(id));
        }
    }
}
