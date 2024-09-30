using CarService.Data;
using CarService.Models;
using CarService.Services.Crud.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CarService.Controllers
{
    public class MasterAdminController : Controller
    {
        private readonly DBContext _context;
        private readonly MyUserService _userService;

        public MasterAdminController(DBContext context, MyUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // GET: Masters
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.OfType<Master>().ToListAsync());
        }

        // GET: Masters/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var master = await _context.Users.OfType<Master>().FirstOrDefaultAsync(m => m.Id == id);
            if (master == null)
            {
                return NotFound();
            }

            return View(master);
        }

        // GET: Masters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Masters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserName,PasswordHash,Name,Surname,PhoneNumber,Email,Specialization")] Master master)
        {
              var passwordHasher = new PasswordHasher<Master>();
                master.PasswordHash = passwordHasher.HashPassword(master, master.PasswordHash);

                _context.Add(master);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }
        
        // GET: Masters/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var master = await _context.Users.OfType<Master>().FirstOrDefaultAsync(m => m.Id == id);
            if (master == null)
            {
                return NotFound();
            }

            return View(master);
        }

        // POST: Masters/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,Name,Surname,PhoneNumber,Email,Specialization")] Master master)
        {
            if (id != master.Id)
            {
                return NotFound();
            }
            
                try
                {
                    var existingMaster = await _context.Users.OfType<Master>().FirstOrDefaultAsync(m => m.Id == id);
                    if (existingMaster == null)
                    {
                        return NotFound();
                    }

                    existingMaster.UserName = master.UserName;
                    existingMaster.Name = master.Name;
                    existingMaster.Surname = master.Surname;
                    existingMaster.PhoneNumber = master.PhoneNumber;
                    existingMaster.Email = master.Email;
                    existingMaster.Specialization = master.Specialization;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Users.Any(e => e.Id == master.Id))
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

        // GET: Masters/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var master = await _context.Users.OfType<Master>().FirstOrDefaultAsync(m => m.Id == id);
            if (master == null)
            {
                return NotFound();
            }

            return View(master);
        }

        // POST: Masters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var master = await _context.Users.OfType<Master>().FirstOrDefaultAsync(m => m.Id == id);
            if (master != null)
            {
                _context.Users.Remove(master);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
