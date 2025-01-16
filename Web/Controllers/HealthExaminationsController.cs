using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain.Domain_Models;
using Repository;

namespace Web.Controllers
{
    public class HealthExaminationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HealthExaminationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HealthExaminations
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.HealthExaminations.Include(h => h.Employee).Include(h => h.Polyclinic);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HealthExaminations/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var healthExamination = await _context.HealthExaminations
                .Include(h => h.Employee)
                .Include(h => h.Polyclinic)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (healthExamination == null)
            {
                return NotFound();
            }

            return View(healthExamination);
        }

        // GET: HealthExaminations/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id");
            ViewData["PolyclinicId"] = new SelectList(_context.Polyclinics, "Id", "Id");
            return View();
        }

        // POST: HealthExaminations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Description,DateTaken,EmployeeId,PolyclinicId,Id")] HealthExamination healthExamination)
        {
            if (ModelState.IsValid)
            {
                healthExamination.Id = Guid.NewGuid();
                _context.Add(healthExamination);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", healthExamination.EmployeeId);
            ViewData["PolyclinicId"] = new SelectList(_context.Polyclinics, "Id", "Id", healthExamination.PolyclinicId);
            return View(healthExamination);
        }

        // GET: HealthExaminations/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var healthExamination = await _context.HealthExaminations.FindAsync(id);
            if (healthExamination == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", healthExamination.EmployeeId);
            ViewData["PolyclinicId"] = new SelectList(_context.Polyclinics, "Id", "Id", healthExamination.PolyclinicId);
            return View(healthExamination);
        }

        // POST: HealthExaminations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Description,DateTaken,EmployeeId,PolyclinicId,Id")] HealthExamination healthExamination)
        {
            if (id != healthExamination.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(healthExamination);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HealthExaminationExists(healthExamination.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", healthExamination.EmployeeId);
            ViewData["PolyclinicId"] = new SelectList(_context.Polyclinics, "Id", "Id", healthExamination.PolyclinicId);
            return View(healthExamination);
        }

        // GET: HealthExaminations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var healthExamination = await _context.HealthExaminations
                .Include(h => h.Employee)
                .Include(h => h.Polyclinic)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (healthExamination == null)
            {
                return NotFound();
            }

            return View(healthExamination);
        }

        // POST: HealthExaminations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var healthExamination = await _context.HealthExaminations.FindAsync(id);
            if (healthExamination != null)
            {
                _context.HealthExaminations.Remove(healthExamination);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HealthExaminationExists(Guid id)
        {
            return _context.HealthExaminations.Any(e => e.Id == id);
        }
    }
}
