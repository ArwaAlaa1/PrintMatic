using Microsoft.AspNetCore.Http;

namespace PrintMatic.Core.Entities
{
    public class CartItems
    {
        public int ProductId { get; set; }

        public string Type { get; set; }
		public string UserId { get; set; }
		
	
		
		public string? Color { get; set; }
		public string? Size { get; set; }
		public string? Text { get; set; }
		public string? Date { get; set; }
		public List<IFormFile?> Photos { get; set; }
		public IFormFile? FilePdf { get; set; }
		
		public int Quantity { get; set; }
    }
}