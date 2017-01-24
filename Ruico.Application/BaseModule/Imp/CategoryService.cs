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
    public class CategoryService : ICategoryService
    {
        ICategoryRepository _Repository;

        #region Constructors

        public CategoryService(ICategoryRepository repository)                               
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            _Repository = repository;
        }

        #endregion

        public CategoryDTO Add(CategoryDTO categoryDTO)
        {
            var category = categoryDTO.ToModel();
            category.Id = IdentityGenerator.NewSequentialGuid();
            category.Created = DateTime.UtcNow;
            if (category.Parent == null || category.Parent.Id == Guid.Empty)
            {
                category.Depth = 1;
            }
            else
            {
                category.Parent = _Repository.Get(categoryDTO.Parent.Id);
                category.Depth = category.Parent.Depth + 1;
            }

            if (category.Name.IsNullOrBlank())
            {
                throw new DefinedException(CommonMessageResources.Name_Empty);
            }

            if (_Repository.Exists(category))
            {
                throw new DataExistsException(string.Format(BaseMessagesResources.Category_Exists_WithValue, category.Name));
            }

            if (category.Parent != null)
            {
                var rulePrefix = category.Parent.ChildSnRulePrefix;
                if (rulePrefix.IsNullOrBlank())
                {
                    throw new DefinedException(BaseMessagesResources.Category_Parent_SerialNumberRule_Prefix_Empty);
                }
                var ruleNumberLength = category.Parent.ChildSnRuleNumberLength;
                if (ruleNumberLength <= 0)
                {
                    throw new DefinedException(BaseMessagesResources.Category_Parent_SerialNumberRule_NumberLength_NotGreat_Than_Zero);
                }
                category.Sn = SerialNumberGenerator.GetSerialNumber(rulePrefix, false, ruleNumberLength);
            }
            else
            {
                var ruleName = CommonMessageResources.SerialNumberRule_Category;
                var snRule = SerialNumberRuleQuerier.FindBy(ruleName);
                if (snRule == null)
                {
                    throw new DataNotFoundException(string.Format(BaseMessagesResources.SerialNumberRule_NotExists_WithValue, ruleName));
                }
                category.Sn = SerialNumberGenerator.GetSerialNumber(snRule.Prefix, snRule.UseDateNumber, snRule.NumberLength);
            }
            _Repository.Add(category);

            #region 操作日志

            var categoryDto = category.ToDto();

            OperateRecorder.RecordOperation(categoryDto.Sn,
                BaseMessagesResources.Add_Category,
                categoryDto.GetOperationLog());

            #endregion

            //commit the unit of work
            _Repository.UnitOfWork.Commit();

            return category.ToDto();
        }

        public void Update(CategoryDTO categoryDTO)
        {
            //get persisted item
            var category = _Repository.Get(categoryDTO.Id);

            if (category == null)
            {
                throw new DataNotFoundException(BaseMessagesResources.Category_NotExists);
            }

            var oldDTO = category.ToDto();

            var current = categoryDTO.ToModel();

            category.Name = current.Name;
            if (current.Parent == null || current.Parent.Id == Guid.Empty)
            {
                category.Depth = 1;
            }
            else
            {
                category.Parent = _Repository.Get(current.Parent.Id);
                category.Depth = current.Parent.Depth + 1;
            }
            category.SortOrder = current.SortOrder;
            category.ChildSnRulePrefix = current.ChildSnRulePrefix;
            category.ChildSnRuleNumberLength = current.ChildSnRuleNumberLength;

            if (category.Name.IsNullOrBlank())
            {
                throw new DefinedException(CommonMessageResources.Name_Empty);
            }

            if (_Repository.Exists(category))
            {
                throw new DataExistsException(string.Format(BaseMessagesResources.Category_Exists_WithValue, category.Name));
            }

            #region 操作日志

            var categoryDto = category.ToDto();

            OperateRecorder.RecordOperation(categoryDto.Sn,
                BaseMessagesResources.Update_Category,
                categoryDto.GetOperationLog(oldDTO));

            #endregion

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public void Remove(Guid id)
        {
            var category = _Repository.Get(id);

            if (category == null)
            {
                throw new DataNotFoundException(BaseMessagesResources.Category_NotExists);
            }

            var categoryDto = category.ToDto();

            _Repository.Remove(category);

            #region 操作日志

            OperateRecorder.RecordOperation(categoryDto.Sn,
                BaseMessagesResources.Remove_Category,
                categoryDto.GetOperationLog());

            #endregion

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public CategoryDTO FindBy(Guid id)
        {
            var category = _Repository.Get(id);

            if (category == null)
            {
                throw new DataNotFoundException(BaseMessagesResources.Category_NotExists);
            }

            return category.ToDto();
        }

        public CategoryDTO FindBySn(string sn)
        {
            var category = _Repository.Find(x => x.Sn == sn);

            if (category == null)
            {
                throw new DataNotFoundException(BaseMessagesResources.Category_NotExists);
            }

            return category.ToDto();
        }

        public CategoryDTO FindByName(string name)
        {
            var category = _Repository.Find(x => x.Name == name);

            if (category == null)
            {
                throw new DataNotFoundException(BaseMessagesResources.Category_NotExists);
            }

            return category.ToDto();
        }

        public IPagedList<CategoryDTO> FindBy(string name, int depth, Guid? parentId, int pageNumber, int pageSize)
        {
            var list = _Repository.FindBy(name, depth, parentId, pageNumber, pageSize);

            var result = list.OrderBy(x => x.SortOrder).ToList();

            return new StaticPagedList<CategoryDTO>(
                result.Select(x => x.ToDto()),
                pageNumber,
                pageSize,
                list.TotalItemCount);
        }
    }
}
