using Microsoft.AspNetCore.Mvc;

namespace ibillcraft.Controllers
{
	public class AboutController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
