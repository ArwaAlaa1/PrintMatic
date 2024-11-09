using Microsoft.AspNetCore.Http;

namespace PrintMatic.Core.Entities
{
    public class CartItems
    {
		public string Id { get; set; } /*= Guid.NewGuid().ToString();*/
        public int ProductId { get; set; }
		public string ProductName { get; set; }
		public string ImageUrl { get; set; }
		public decimal Price { get; set; }
		public decimal? PriceAfterSale { get; set; }
		public string Type { get; set; }
		
		public string? Color { get; set; }
		public string? Size { get; set; }
		public string? Text { get; set; }
		public string? Date { get; set; }
		public List<string>? Photos { get; set; }
		public string? FilePdf { get; set; }
		
		public int Quantity { get; set; }
        public int MaxData { get; set; }
        public int MinData { get; set; }
        
        public decimal TotalPrice => (decimal)(PriceAfterSale ==0 ? Price * Quantity: PriceAfterSale*Quantity);

        
    }
}