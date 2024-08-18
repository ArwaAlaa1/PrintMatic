using Microsoft.AspNetCore.Mvc;
using PrintMatic.Core.Entities;

namespace PrintMartic_DashBoard.Helper.ViewModels
{
	public class ProductPhotosVM
	{
     
        public string? Photo { get; set; }
        public string? FilePath { get; set; }

        public IFormFile? PhotoFile { get; set; }

		public Product? Product { get; set; }
        //[Remote("CheckUnique" , "ProductPhoto" , AdditionalFields = [])]
		public int ProductId { get; set; }

		public IEnumerable<Product> Products { get; set; } = new List<Product>();
	}
}
