using Microsoft.AspNetCore.Mvc;

namespace ibillcraft.Controllers
{
	public class TermsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
