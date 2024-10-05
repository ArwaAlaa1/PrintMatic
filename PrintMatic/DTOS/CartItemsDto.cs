using System.ComponentModel.DataAnnotations;

namespace PrintMatic.DTOS
{
    public class CartItemsDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ItemName { get; set; }
        [Required]
        public string PhotoURL { get; set; }
        [Required]
        [Range(0.1,double.MaxValue,ErrorMessage ="Price must be greater than Zero")]
        public decimal Price { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than Zero")]
        public int Quantity { get; set; }
    }
}