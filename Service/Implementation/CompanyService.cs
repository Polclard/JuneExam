using Domain.Domain_Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
    public class CompanyService : ICompanyService
    {

        private readonly IRepository<Company> _companyRepository;

        public CompanyService(IRepository<Company> repository)
        {
            _companyRepository = repository;
        }

        public void CreateNewCompany(Company p)
        {
            _companyRepository.Insert(p);
        }

        public void DeleteCompany(Guid id)
        {
            _companyRepository.Delete(GetDetailsForCompany(id));
        }

        public List<Company> GetAllCompanies()
        {
            return _companyRepository.GetAll().ToList();
        }

        public Company GetDetailsForCompany(Guid? id)
        {
            return _companyRepository.Get(id);
        }

        public void UpdateExistingCompany(Company p)
        {
            _companyRepository.Update(p);
        }
    }
}
