using System;
using System.Linq;
using PagedList;
using Ruico.Application.Exceptions;
using Ruico.Application.Extensions;
using Ruico.Application.Resources.Generated;
using Ruico.Application.SystemModule.Imp;
using Ruico.Domain.BaseModule.Repositories;
using Ruico.Dto.Base;
using Ruico.Dto.Base.Converters;
using Ruico.Infrastructure.Entity;
using Ruico.Infrastructure.Utility.Helper;

namespace Ruico.Application.BaseModule.Imp
{
    public class SerialNumberRuleService : ISerialNumberRuleService
    {
        ISerialNumberRuleRepository _Repository;

        #region Constructors

        public SerialNumberRuleService(ISerialNumberRuleRepository repository)                               
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            _Repository = repository;
        }

        #endregion

        public SerialNumberRuleDTO Add(SerialNumberRuleDTO SerialNumberRuleDTO)
        {
            var serialNumberRule = SerialNumberRuleDTO.ToModel();
            serialNumberRule.Id = IdentityGenerator.NewSequentialGuid();
            serialNumberRule.Created = DateTime.UtcNow;

            if (serialNumberRule.RuleName.IsNullOrBlank())
            {
                throw new DefinedException(CommonMessageResources.Name_Empty);
            }

            if (serialNumberRule.Prefix.IsNullOrBlank())
            {
                throw new DefinedException(CommonMessageResources.Prefix_Empty);
            }

            if (serialNumberRule.NumberLength <= 0)
            {
                throw new DefinedException(BaseMessagesResources.NumberLength_NotGreat_Than_Zero);
            }

            if (_Repository.Exists(serialNumberRule))
            {
                throw new DataExistsException(string.Format(BaseMessagesResources.SerialNumberRule_Exists, serialNumberRule.RuleName));
            }

            _Repository.Add(serialNumberRule);

            #region 操作日志

            var serialNumberRuleDto = serialNumberRule.ToDto();

            OperateRecorder.RecordOperation(serialNumberRuleDto.Id.ToString(),
                BaseMessagesResources.Add_SerialNumberRule,
                serialNumberRuleDto.GetOperationLog());

            #endregion

            //commit the unit of work
            _Repository.UnitOfWork.Commit();

            return serialNumberRule.ToDto();
        }

        public void Update(SerialNumberRuleDTO SerialNumberRuleDTO)
        {
            //get persisted item
            var serialNumberRule = _Repository.Get(SerialNumberRuleDTO.Id);

            if (serialNumberRule == null)
            {
                throw new DataNotFoundException(BaseMessagesResources.SerialNumberRule_NotExists);
            }

            var oldDTO = serialNumberRule.ToDto();

            var current = SerialNumberRuleDTO.ToModel();

            serialNumberRule.RuleName = current.RuleName;
            serialNumberRule.Prefix = current.Prefix;
            serialNumberRule.UseDateNumber = current.UseDateNumber;
            serialNumberRule.NumberLength = current.NumberLength;

            if (serialNumberRule.RuleName.IsNullOrBlank())
            {
                throw new DefinedException(CommonMessageResources.Name_Empty);
            }

            if (_Repository.Exists(serialNumberRule))
            {
                throw new DataExistsException(string.Format(BaseMessagesResources.SerialNumberRule_Exists, serialNumberRule.RuleName));
            }

            #region 操作日志

            var ruleDto = serialNumberRule.ToDto();

            OperateRecorder.RecordOperation(oldDTO.Id.ToString(),
                BaseMessagesResources.Update_SerialNumberRule,
                ruleDto.GetOperationLog(oldDTO));

            #endregion

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public void Remove(Guid id)
        {
            var serialNumberRule = _Repository.Get(id);

            if (serialNumberRule == null)
            {
                throw new DataNotFoundException(BaseMessagesResources.SerialNumberRule_NotExists);
            }

            var serialNumberRuleDto = serialNumberRule.ToDto();

            _Repository.Remove(serialNumberRule);

            #region 操作日志

            OperateRecorder.RecordOperation(serialNumberRuleDto.Id.ToString(),
                BaseMessagesResources.Remove_SerialNumberRule,
                serialNumberRuleDto.GetOperationLog());

            #endregion

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public SerialNumberRuleDTO FindBy(Guid id)
        {
            return _Repository.Get(id).ToDto();
        }

        public SerialNumberRuleDTO FindBy(string name)
        {
            return _Repository.Find(x => x.RuleName == name).ToDto();
        }

        public IPagedList<SerialNumberRuleDTO> FindBy(string name, int pageNumber, int pageSize)
        {
            var list = _Repository.FindBy(name, pageNumber, pageSize);

            var result = list.OrderBy(x => x.RuleName).ToList();

            return new StaticPagedList<SerialNumberRuleDTO>(
                result.Select(x => x.ToDto()),
                pageNumber,
                pageSize,
                list.TotalItemCount);
        }
    }
}
