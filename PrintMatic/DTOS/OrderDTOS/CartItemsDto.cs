using System.ComponentModel.DataAnnotations;

namespace PrintMatic.DTOS.OrderDTOS
{
    public class CartItemsDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int ProductId { get; set; }
        //public string ProductName { get; set; }
        //public string ImageUrl { get; set; }
        //public decimal Price { get; set; }
        //public decimal? PriceAfterSale { get; set; } = 0.0M;
        public string Type { get; set; }
      
        public string? Color { get; set; }
        public string? Size { get; set; }
        public string? Text { get; set; }
        public string? Date { get; set; }
        public List<IFormFile>? Photos { get; set; }
        public IFormFile? FilePdf { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "يجب ان تكون الكميه اكبر من الصفر")]
        public int Quantity { get; set; }
        
        //public decimal TotalPrice => (decimal)(PriceAfterSale == 0 ? Price * Quantity : PriceAfterSale * Quantity);






    }
}