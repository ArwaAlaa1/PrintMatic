using Microsoft.AspNetCore.Mvc;

namespace PrintMatic.Controllers
{
	[ApiController]
	[Route("/error")]
	public class ErrorController : ControllerBase
	{
		[HttpGet]
		public IActionResult HandleError() =>
			Problem("An unexpected error occurred, please try again later.");
	}

}
