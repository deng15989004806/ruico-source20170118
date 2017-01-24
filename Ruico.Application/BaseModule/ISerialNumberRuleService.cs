using System;
using PagedList;
using Ruico.Dto.Base;

namespace Ruico.Application.BaseModule
{
    public interface ISerialNumberRuleService
    {
        SerialNumberRuleDTO Add(SerialNumberRuleDTO menuDTO);

        void Update(SerialNumberRuleDTO menuDTO);

        void Remove(Guid id);

        SerialNumberRuleDTO FindBy(Guid id);

        SerialNumberRuleDTO FindBy(string name);

        IPagedList<SerialNumberRuleDTO> FindBy(string name, int pageNumber, int pageSize);
    }
}
