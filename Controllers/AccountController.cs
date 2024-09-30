using CarService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CarService.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
                var user = new User { UserName = model.UserName, Email = model.Email, Name = model.Name, Surname = model.Surname };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Redirect("http://localhost:5230/");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            return View(model);
        }
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);

                    if (user != null)
                    {
                        if (model.UserName.Equals("maxym", StringComparison.OrdinalIgnoreCase))
                        {
                            return RedirectToAction("Index", "User");
                        }
                        if (model.UserName.Equals("master", StringComparison.OrdinalIgnoreCase))
                        {
                            return RedirectToAction("Index", "Master");
                        }
                        if (model.UserName.Equals("admin", StringComparison.OrdinalIgnoreCase))
                        {
                            return RedirectToAction("Index", "MainAdmin");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Невідома роль користувача.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Користувача не знайдено.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Невірний логін або пароль.");
                }

            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account"); 
        }

    }
}