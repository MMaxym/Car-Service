using Microsoft.AspNetCore.Mvc;

namespace CarService.Controllers
{
    public class MainAdminController : Controller
    {
        // GET: MainAdmin
        public IActionResult Index()
        {
            return View();
        }
    }
}