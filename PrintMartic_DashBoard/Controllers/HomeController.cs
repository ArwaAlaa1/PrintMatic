using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintMartic_DashBoard.Models;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Repository.Contract;
using System.Diagnostics;

namespace PrintMartic_DashBoard.Controllers
{
    [Authorize(AuthenticationSchemes = "Cookies")]
    public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly IProdduct _product;

        public HomeController(ILogger<HomeController> logger , IProdduct product)
		{
			_logger = logger;
            _product = product;
        }
		[Authorize(AuthenticationSchemes = "Cookies")]
		public async Task<IActionResult> Index()
		{
            bool flag = false;
            if (User.IsInRole("Admin"))
			{
                var list = await _product.GetWaitingProducts();
                if (list.Any())
                {
                    flag = true;
                }
                return View(flag);
            }
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
