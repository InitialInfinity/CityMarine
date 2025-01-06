using Microsoft.AspNetCore.Mvc;

namespace ibillcraft.Controllers
{
	public class SettingController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult OfferMessages()
		{
			return View();
		}
		public IActionResult Email()
		{
			return View();
		}
	}
}
