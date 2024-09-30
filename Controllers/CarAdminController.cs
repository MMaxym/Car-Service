using CarService.Data;
using CarService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CarService.Controllers
{
    public class CarAdminController : Controller
    {
        private readonly DBContext _context;

        public CarAdminController(DBContext context)
        {
            _context = context;
        }

        // GET: Cars
        public async Task<IActionResult> Index()
        {
            var cars = await _context.Cars.Include(c => c.User).ToListAsync();
            return View(cars);
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int id)
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

        // GET: Cars/Create
        public IActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: Cars/Create
        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Make,Model,Year,Number,Vin,Mileage,UserId")] Car car)
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
            return RedirectToAction(nameof(Index));
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
    
            PopulateDropdowns();
            return View(car);
        }

        // POST: Cars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Make,Model,Year,Number,Vin,Mileage,UserId")] Car car)
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
            return RedirectToAction(nameof(Index));
        }



        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int id)
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

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }
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

    }
}
