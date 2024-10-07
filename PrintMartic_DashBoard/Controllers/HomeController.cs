using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintMartic_DashBoard.Models;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using System.Diagnostics;

namespace PrintMartic_DashBoard.Controllers
{
    [Authorize(AuthenticationSchemes = "Cookies")]
    public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork<Product> _unitOfWork;

        public HomeController(ILogger<HomeController> logger , IUnitOfWork<Product> unitOfWork)
		{
			_logger = logger;
            _unitOfWork = unitOfWork;
        }
		[Authorize(AuthenticationSchemes = "Cookies")]
		public async Task<IActionResult> Index()
		{
            bool flag = false;
            if (User.IsInRole("Admin"))
			{
                var list = await _unitOfWork.prodduct.GetWaitingProducts();
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
