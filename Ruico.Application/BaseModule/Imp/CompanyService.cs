using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using Ruico.Application.Exceptions;
using Ruico.Application.Extensions;
using Ruico.Application.Resources.Generated;
using Ruico.Application.SystemModule.Imp;
using Ruico.Domain.BaseModule.Entities;
using Ruico.Domain.BaseModule.Repositories;
using Ruico.Dto.Base;
using Ruico.Dto.Base.Converters;
using Ruico.Infrastructure.Entity;
using Ruico.Infrastructure.Utility.Helper;

namespace Ruico.Application.BaseModule.Imp
{
    public class CompanyService : ICompanyService
    {
        ICompanyRepository _Repository;
        ICategoryRepository _CategoryRepository;

        #region Constructors

        public CompanyService(ICompanyRepository repository,
            ICategoryRepository categoryRepository)                               
        {
            if (repository == null)
                throw new ArgumentNullException("repository");

            _Repository = repository;
            _CategoryRepository = categoryRepository;
        }

        #endregion

        public CompanyDTO Add(CompanyDTO companyDTO)
        {
            var company = companyDTO.ToModel();
            company.Id = IdentityGenerator.NewSequentialGuid();
            company.Created = DateTime.UtcNow;
            if (companyDTO.Category != null)
            {
                company.Category = _CategoryRepository.Get(companyDTO.Category.Id);
            }
            else
            {
                company.Category = null;
            }

            if (company.Name.IsNullOrBlank())
            {
                throw new DefinedException(CommonMessageResources.Name_Empty);
            }

            if (_Repository.Exists(company))
            {
                throw new DataExistsException(string.Format(BaseMessagesResources.Company_Exists_WithValue, company.Name));
            }

            var ruleName = CommonMessageResources.SerialNumberRule_Company;
            var snRule = SerialNumberRuleQuerier.FindBy(ruleName);
            if (snRule == null)
            {
                throw new DataNotFoundException(string.Format(BaseMessagesResources.SerialNumberRule_NotExists_WithValue, ruleName));
            }
            company.Sn = SerialNumberGenerator.GetSerialNumber(snRule.Prefix, snRule.UseDateNumber, snRule.NumberLength);

            _Repository.Add(company);

            #region 操作日志

            var companyDto = company.ToDto();

            OperateRecorder.RecordOperation(companyDto.Sn,
                BaseMessagesResources.Add_Company,
                companyDto.GetOperationLog());

            #endregion

            //commit the unit of work
            _Repository.UnitOfWork.Commit();

            return company.ToDto();
        }

        public void Update(CompanyDTO companyDTO)
        {
            //get persisted item
            var company = _Repository.Get(companyDTO.Id);

            if (company == null)
            {
                throw new DataNotFoundException(BaseMessagesResources.Company_NotExists);
            }

            var oldDTO = company.ToDto();

            var current = companyDTO.ToModel();

            company.Name = current.Name;
            company.Contact = current.Contact;
            company.Phone = current.Phone;
            company.Fax = current.Fax;
            company.Mobile = current.Mobile;
            company.Address = current.Address;
            company.Postcode = current.Postcode;
            company.Email = current.Email;
            if (companyDTO.Category != null)
            {
                company.Category = _CategoryRepository.Get(companyDTO.Category.Id);
            }
            else
            {
                company.Category = null;
            }

            if (company.Name.IsNullOrBlank())
            {
                throw new DefinedException(CommonMessageResources.Name_Empty);
            }

            if (_Repository.Exists(company))
            {
                throw new DataExistsException(string.Format(BaseMessagesResources.Company_Exists_WithValue, company.Name));
            }

            #region 操作日志

            var companyDto = company.ToDto();

            OperateRecorder.RecordOperation(companyDto.Sn,
                BaseMessagesResources.Update_Company,
                companyDto.GetOperationLog(oldDTO));

            #endregion

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public void Remove(Guid id)
        {
            var company = _Repository.Get(id);

            if (company == null)
            {
                throw new DataNotFoundException(BaseMessagesResources.Company_NotExists);
            }

            var companyDto = company.ToDto();

            _Repository.Remove(company);

            #region 操作日志

            OperateRecorder.RecordOperation(companyDto.Sn,
                BaseMessagesResources.Remove_Company,
                companyDto.GetOperationLog());

            #endregion

            //commit unit of work
            _Repository.UnitOfWork.Commit();
        }

        public CompanyDTO FindBy(Guid id)
        {
            var company = _Repository.Get(id);

            if (company == null)
            {
                throw new DataNotFoundException(BaseMessagesResources.Company_NotExists);
            }

            return company.ToDto();
        }

        public IPagedList<CompanyDTO> FindBy(string name, Guid? categoryId, int pageNumber, int pageSize)
        {
            var list = _Repository.FindBy(name, categoryId, pageNumber, pageSize);

            var result = list.ToList();

            return new StaticPagedList<CompanyDTO>(
                result.Select(x => x.ToDto()),
                pageNumber,
                pageSize,
                list.TotalItemCount);
        }

        public IList<CategoryDTO> GetCompanyCategories()
        {
            var category = _CategoryRepository.Find(x => x.Depth == 1 && x.Name == CommonMessageResources.Category_Company);

            return category == null ? new List<CategoryDTO>() : 
                (category.Children ?? new List<Category>()).Select(x => x.ToDto()).ToList();
        }
    }
}
