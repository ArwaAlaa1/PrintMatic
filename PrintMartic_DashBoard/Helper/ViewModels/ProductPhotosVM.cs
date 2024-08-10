using PrintMatic.Core.Entities;

namespace PrintMartic_DashBoard.Helper.ViewModels
{
	public class ProductPhotosVM
	{
		public string? Photo { get; set; }
		public string? PathPhoto { get; set; }
		public IFormFile? PhotoFile { get; set; }

		public Product? Product { get; set; }
		[UniquePRP]
		public int ProductId { get; set; }

		public IEnumerable<Product> Products { get; set; } = new List<Product>();
	}
}
