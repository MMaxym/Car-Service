using CarService.Data;
using CarService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarService.Controllers
{
    public class UserController : Controller
    {
        private readonly DBContext _context;

        public UserController(DBContext context)
        {
            _context = context;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> MyCars()
        {
            var userName = "maxym"; 
            var cars = await _context.Cars
                .Include(c => c.User) 
                .Where(c => c.User.UserName == userName)
                .ToListAsync();
    
            return View(cars);
        }
        
        public IActionResult CreateCar()
        {
            PopulateDropdowns();
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCar([Bind("Make,Model,Year,Number,Vin,Mileage,UserId")] Car car)
        {
            if (car.UserId == null)
            {
                ModelState.AddModelError("UserId", "Будь ласка, виберіть власника.");
                PopulateDropdowns();
                return View(car);
            }
            
            var user = await _context.Users.FindAsync(car.UserId);
            if (user == null)
            {
                ModelState.AddModelError("UserId", "Користувача не знайдено.");
                PopulateDropdowns();
                return View(car);
            }
            
            _context.Add(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyCars));
        }
        
        public async Task<IActionResult> EditCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
    
            PopulateDropdowns();
            return View(car);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCar(int id, [Bind("Id,Make,Model,Year,Number,Vin,Mileage,UserId")] Car car)
        {
            if (id != car.Id)
            {
                return NotFound();
            }
            
            if (car.UserId == null)
            {
                ModelState.AddModelError("UserId", "Будь ласка, виберіть власника.");
                PopulateDropdowns();
                return View(car);
            }
            
            try
            {
                _context.Update(car);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Cars.Any(e => e.Id == car.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(MyCars));
        }
        
        public async Task<IActionResult> DetailsCar(int id)
        {
            var car = await _context.Cars
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }
        
        public async Task<IActionResult> DeleteCar(int id)
        {
            var car = await _context.Cars
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCarConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(MyCars));
        }
        
        public async Task<IActionResult> MyRepairs(string status)
        {
            var userName = "maxym";
            var repairRecords = await _context.RepairRecords
                .Include(r => r.Car)   
                .ThenInclude(c => c.User) 
                .Include(r => r.Master)  
                .Where(r => r.Car.User.UserName == userName)
                .ToListAsync();
            
            
            if (!string.IsNullOrEmpty(status))
            {
                repairRecords = repairRecords.Where(r => r.Status == status).ToList();
            }
            
            foreach (var record in repairRecords)
            {
                record.Cost = Math.Round(record.Cost, 2);
            }

            return View(repairRecords);
        }
        
        public async Task<IActionResult> DetailsRepair(int id)
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
        public IActionResult CreateRepair()
        {
            PopulateDropdowns2();
            return View();
        }

        // POST: RepairRecords/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRepair([Bind("ScheduledDate,Status,RepairDescription,Cost,CarId,MasterId")] RepairRecord repairRecord)
        {
            _context.Add(repairRecord);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
                
        }
        
        private void PopulateDropdowns()
        {
            var users = _context.Users
                .Select(u => new
                {
                    Id = u.Id,
                    UserName = u.UserName
                }).ToList();

            ViewBag.Users = new SelectList(users, "Id", "UserName");
        }
        
        private void PopulateDropdowns2()
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