using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain.Domain_Models;
using Repository;
using Service.Interface;
using Service.Implementation;
using Domain.DTO;

namespace Web.Controllers
{
    public class EmployeesController : Controller
    {

        private readonly IPolyclinicService _polyclinicService;
        private readonly ICompanyService _companyService;
        private readonly IEmployeeService _employeeService;
        private readonly IHealthExaminationService _healthExaminationService;
        private readonly ApplicationDbContext _context;

        public EmployeesController(IEmployeeService employeeService,
            ApplicationDbContext applicationDbContext,
            IHealthExaminationService healthExaminationService,
            ICompanyService companyService,
            IPolyclinicService polyclinicService)
        {
            _employeeService = employeeService;
            _context = applicationDbContext;
            _polyclinicService = polyclinicService;
            _companyService = companyService;
            _healthExaminationService = healthExaminationService;
            loadCompanyAndHealthExaminations();
        }

        public IActionResult AddAppointment(Guid ? id)
        {
            var employee = _employeeService.GetDetailsForEmployee(id);

            var exam = new HealthExaminationDTO
            {
                EmployeeId = employee.Id
            };

            ViewData["Clinics"] = _polyclinicService.GetAllPolyclinics();
            var company = _companyService.GetAllCompanies().Where(c => c.ListOfEmployees.Any(r => r.Id == exam.EmployeeId)).FirstOrDefault();
            ViewData["CompanyName"] = company.CompanyName;
            return View(exam);
        }

        [HttpPost]
        public IActionResult AddExamination(HealthExaminationDTO healthExaminationDTO)
        {
            if(_polyclinicService.GetDetailsForPolyclinic(healthExaminationDTO.PolyclinicId).AvailableSlots <= 1)
            {
                return RedirectToAction("FullClinic");
            }

            _healthExaminationService.CreateNewHealthExamination
            (
                new HealthExamination
                {
                    Id = Guid.NewGuid(),
                    Description = healthExaminationDTO.Description,
                    DateTaken = healthExaminationDTO.DateTaken,
                    EmployeeId = healthExaminationDTO.EmployeeId,
                    PolyclinicId = healthExaminationDTO.PolyclinicId,
                    Employee = _employeeService.GetDetailsForEmployee(healthExaminationDTO.EmployeeId),
                    Polyclinic = _polyclinicService.GetDetailsForPolyclinic(healthExaminationDTO.PolyclinicId)

                }
            );

            _polyclinicService.GetDetailsForPolyclinic(healthExaminationDTO.PolyclinicId).AvailableSlots--;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult FullClinic()
        {
            return View();
        }

        public void loadCompanyAndHealthExaminations()
        {
            foreach(var employee in _employeeService.GetAllEmployees())
            {
                employee.Company = _context.Companies.FirstOrDefault(z => z.Id == employee.CompanyId);
                List<HealthExamination> healthExaminations = new List<HealthExamination>();
                foreach(var healthExamination in _context.HealthExaminations.AsEnumerable().ToList())
                {
                    if(healthExamination.EmployeeId.Equals(employee.Id))
                    {
                        healthExaminations.Add(healthExamination);
                    }
                }
                foreach (var healthExamination in healthExaminations)
                {
                    foreach (var polyclinc in _context.Polyclinics.AsEnumerable().ToList())
                    {
                        if(healthExamination.PolyclinicId.Equals(polyclinc.Id))
                        {
                            healthExamination.Polyclinic = polyclinc;
                        }
                    }
                }
                employee.HealthExaminations = healthExaminations;
            }
        }

        // GET: Employees
        public IActionResult Index()
        {
            var a = _employeeService.GetAllEmployees();
            return View(a);
        }

        // GET: Employees/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = _employeeService.GetDetailsForEmployee(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id");
            ViewData["CompanyName"] = new SelectList(_context.Companies, "CompanyName", "CompanyName");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FullName,DateOfBirth,Title,CompanyId,Id")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.Id = Guid.NewGuid();
                _employeeService.CreateNewEmployee(employee);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id", employee.CompanyId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = _employeeService.GetDetailsForEmployee(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id", employee.CompanyId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("FullName,DateOfBirth,Title,CompanyId,Id")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _employeeService.UpdateExistingEmployee(employee);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Id", employee.CompanyId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = _employeeService.GetDetailsForEmployee(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var employee = _employeeService.GetDetailsForEmployee(id);
            if (employee != null)
            {
                _employeeService.DeleteEmployee(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(Guid id)
        {
            return _employeeService.GetDetailsForEmployee(id) != null;
        }


        public IActionResult AddEmployeeToHealthExamination()
        {
            return View();
        }

    }
}
