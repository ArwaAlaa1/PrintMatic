using PrintMatic.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace PrintMatic.DTOS
{
    public class CustomerCartDto
    {
        [Required]
        public string Id { get; set; }
        public List<CartItemsDto> Items { get; set; }
    }
}
