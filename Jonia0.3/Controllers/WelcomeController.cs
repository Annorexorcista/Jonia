using Microsoft.AspNetCore.Mvc;

namespace Jonia0._3.Controllers
{
    public class WelcomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
