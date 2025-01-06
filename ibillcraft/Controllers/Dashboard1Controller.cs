using Microsoft.AspNetCore.Mvc;

namespace ibillcraft.Controllers
{
	public class Dashboard1Controller : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
