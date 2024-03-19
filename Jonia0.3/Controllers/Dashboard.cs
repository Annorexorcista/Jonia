using Microsoft.AspNetCore.Mvc;

namespace Jonia0._2.Controllers
{
	public class Dashboard : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
