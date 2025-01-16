using Domain.Domain_Models;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeeService(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public void CreateNewEmployee(Employee p)
        {
            _employeeRepository.Insert(p);
        }

        public void DeleteEmployee(Guid id)
        {
            _employeeRepository.Delete(GetDetailsForEmployee(id));
        }

        public List<Employee> GetAllEmployees()
        {
            return _employeeRepository.GetAll().ToList();
        }

        public Employee GetDetailsForEmployee(Guid? id)
        {
            return _employeeRepository.Get(id);
        }

        public void UpdateExistingEmployee(Employee p)
        {
            _employeeRepository.Update(p);
        }
    }
}
