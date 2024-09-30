using CarService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarService.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarService.Controllers
{
    public class MasterController : Controller
    {
        private readonly DBContext _context;

        public MasterController(DBContext context)
        {
            _context = context;
        }

        // GET: Master
        public async Task<IActionResult> Index(string status, string search)
        {
            var repairRecords = await _context.RepairRecords
                .Include(r => r.Car)
                .Include(r => r.Master)
                .Where(r => r.Master.UserName == "master") 
                .ToListAsync();

            if (!string.IsNullOrEmpty(status))
            {
                repairRecords = repairRecords.Where(r => r.Status == status).ToList();
            }

            if (!string.IsNullOrEmpty(search))
            {
                repairRecords = repairRecords.Where(r => r.Car.Number.Contains(search)).ToList();
            }

            foreach (var record in repairRecords)
            {
                record.Cost = Math.Round(record.Cost, 2);
            }

            return View(repairRecords);
        }

        // GET: Master/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var repairRecord = await _context.RepairRecords
                .Include(r => r.Car)
                .Include(r => r.Master)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (repairRecord == null)
            {
                return NotFound();
            }

            return View(repairRecord);
        }

        // GET: Master/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var repairRecord = await _context.RepairRecords.FindAsync(id);
            if (repairRecord == null)
            {
                return NotFound();
            }

            repairRecord.Cost = Math.Round(repairRecord.Cost, 2);
            PopulateDropdowns();

            return View(repairRecord);
        }

        // POST: Master/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Status")] RepairRecord repairRecord)
        {
            if (id != repairRecord.Id)
            {
                return NotFound();
            }
            
            var recordToUpdate = await _context.RepairRecords.FindAsync(id);
            if (recordToUpdate == null)
            {
                return NotFound();
            }
            
            recordToUpdate.Status = repairRecord.Status;

            try
            {
                _context.Update(recordToUpdate);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RepairRecordExists(repairRecord.Id))
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

        private bool RepairRecordExists(int id)
        {
            return _context.RepairRecords.Any(e => e.Id == id);
        }

        private void PopulateDropdowns()
        {
            ViewBag.Cars = new SelectList(_context.Cars.Select(car => new
            {
                Id = car.Id,
                Name = car.Make + " " + car.Model 
            }), "Id", "Name");
            
            ViewBag.Masters = new SelectList(_context.Masters.Select(master => new
            {
                Id = master.Id,
                FullName = master.Surname + " " + master.Name + " (" + master.Specialization + ")" 
            }), "Id", "FullName");
            
            ViewBag.Statuses = new SelectList(new List<string>
            {
                "В обробці",   
                "Завершено",   
                "Скасовано"    
            });
        }
    }
}
