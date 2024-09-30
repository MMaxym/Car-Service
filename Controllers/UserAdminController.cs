using Microsoft.AspNetCore.Mvc;
using CarService.Data;
using CarService.Interfaces;
using CarService.Models;
using CarService.Services.Crud.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI;

namespace CarService.Controllers;

public class UserAdminController : Controller
{
    private readonly DBContext _context;
    private readonly MyUserService _userService;

    public UserAdminController(DBContext context, MyUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _userService.GetAllAsync());
    }
     public async Task<IActionResult> Details(string id)
        {
            var client = await _userService.GetUserByLoginAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

    // GET: Clients/Create
    public IActionResult Create()
    {
        ViewData["ClientId"] = new SelectList(_context.Users, "Id", "Id");
        ViewData["ClientId"] = new SelectList(_context.Users, "Id", "Id");
        return View();
    }

    // POST: Clients/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("UserName,PasswordHash,Name,Surname,PhoneNumber,Email")] User user)
    {
        
        var passwordHasher = new PasswordHasher<User>();
        user.PasswordHash = passwordHasher.HashPassword(user, user.PasswordHash);

        _context.Add(user);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
        
        ViewData["ClientId"] = new SelectList(_context.Users, "Id", "Id", user.Id);
        return View(user);
    }

    // GET: Clients/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var client = await _context.Users.FindAsync(id);
        if (client == null)
        {
            return NotFound();
        }
        ViewData["ClientId"] = new SelectList(_context.Users, "Id", "Id", client.Id);
        return View(client);
    }

    // POST: Clients/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,PasswordHash,Name,Surname,PhoneNumber,Email")] User user)
    {
        if (id != user.Id)
        {
            return NotFound();
        }

        
            try
            {
                var existingClient = await _context.Users.FindAsync(id);
                
                existingClient.UserName = user.UserName;
                existingClient.Name = user.Name;
                existingClient.Surname = user.Surname;
                existingClient.PhoneNumber = user.PhoneNumber;
                existingClient.Email = user.Email;
                /*_context.Update(client);*/
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(user.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        
        ViewData["ClientId"] = new SelectList(_context.Users, "Id", "Id", user.Id);
        return View(user);
    }

    // GET: Clients/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var client = await _context.Users
            .FirstOrDefaultAsync(m => m.Id == id);
        if (client == null)
        {
            return NotFound();
        }

        return View(client);
    }

    // POST: Clients/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var client = await _context.Users.FindAsync(id);
        if (client != null)
        {
            _context.Users.Remove(client);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ClientExists(string id)
    {
        return _context.Users.Any(e => e.Id == id);
    }
}