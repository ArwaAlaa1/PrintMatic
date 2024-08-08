using Microsoft.AspNetCore.Mvc;

namespace PrintMartic_DashBoard.Controllers
{
	public class UserAccountController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
