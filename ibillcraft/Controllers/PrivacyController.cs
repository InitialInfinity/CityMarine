using Microsoft.AspNetCore.Mvc;

namespace ibillcraft.Controllers
{
	public class PrivacyController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
