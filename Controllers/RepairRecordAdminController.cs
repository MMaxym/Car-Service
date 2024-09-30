using CarService.Data;
using CarService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarService.Controllers
{
    public class RepairRecordAdminController : Controller
    {
        private readonly DBContext _context;

        public RepairRecordAdminController(DBContext context)
        {
            _context = context;
        }

        // GET: RepairRecords
        public async Task<IActionResult> Index()
        {
            var repairRecords = await _context.RepairRecords
                .Include(r => r.Car)
                .Include(r => r.Master)
                .ToListAsync();
            
            foreach (var record in repairRecords)
            {
                record.Cost = Math.Round(record.Cost, 2);
            }

            return View(repairRecords);
        }


        // GET: RepairRecords/Details/5
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

        // GET: RepairRecords/Create
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: RepairRecords/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScheduledDate,Status,RepairDescription,Cost,CarId,MasterId")] RepairRecord repairRecord)
        {
                _context.Add(repairRecord);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                
        }

        // GET: RepairRecords/Edit/5
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

        // POST: RepairRecords/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ScheduledDate,Status,RepairDescription,Cost,CarId,MasterId,CreatedAt,UpdatedAt")] RepairRecord repairRecord)
        {
            if (id != repairRecord.Id)
            {
                return NotFound();
            }
             try
                {
                    _context.Update(repairRecord);
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

            PopulateDropdowns();
            return View(repairRecord);
        }

        // GET: RepairRecords/Delete/5
        public async Task<IActionResult> Delete(int id)
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

        // POST: RepairRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var repairRecord = await _context.RepairRecords.FindAsync(id);
            if (repairRecord != null)
            {
                _context.RepairRecords.Remove(repairRecord);
                await _context.SaveChangesAsync();
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
