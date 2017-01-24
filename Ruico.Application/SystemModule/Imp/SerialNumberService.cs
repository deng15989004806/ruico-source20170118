using System;
using System.Linq;
using PagedList;
using Ruico.Domain.SystemModule.Repositories;
using Ruico.Dto.System;
using Ruico.Dto.System.Converters;

namespace Ruico.Application.SystemModule.Imp
{
    public class SerialNumberService : ISerialNumberService
    {
        ISerialNumberRepository _Repository;

        #region Constructors

        public SerialNumberService(ISerialNumberRepository repository)                               
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            _Repository = repository;
        }

        #endregion

        public int GetSerialNumber(string prefix, string dateNumber, int increase)
        {
            return _Repository.GetSerialNumber(prefix, dateNumber, increase);
        }

        public IPagedList<SerialNumberDTO> FindBy(int pageNumber, int pageSize)
        {
            var list = _Repository.FindBy(pageNumber, pageSize);

            return new StaticPagedList<SerialNumberDTO>(
                list.ToList().Select(x => x.ToDto()),
                pageNumber,
                pageSize,
                list.TotalItemCount);
        }

        public void Remove(long id)
        {
            _Repository.Remove(id);

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }
    }
}
