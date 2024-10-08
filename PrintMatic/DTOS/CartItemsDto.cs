using System.ComponentModel.DataAnnotations;

namespace PrintMatic.DTOS
{
    public class CartItemsDto
    {
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public string ImageUrl { get; set; }
		public decimal Price { get; set; }
		public decimal PriceAfterSale { get; set; }
		public string Type { get; set; }
		public string UserId { get; set; }

		public string? Color { get; set; }
		public string? Size { get; set; }
		public string? Text { get; set; }
		public string? Date { get; set; }
		public List<IFormFile>? Photos { get; set; }
		public IFormFile? FilePdf { get; set; }
		[Required]
		[Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than Zero")]

		public int Quantity { get; set; }
		
        

		



	}
}