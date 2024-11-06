using PrintMatic.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace PrintMatic.DTOS.OrderDTOS
{
    public class CustomerCartDto
    {

        public string? Id { get; set; }
        public List<CartItemsDto> Items { get; set; }
        public bool IsProductUnique(CartItemsDto newItem)
        {
            return !Items.Any(existingItem =>
                existingItem.ProductId == newItem.ProductId &&
                existingItem.ProductName == newItem.ProductName &&
                existingItem.ImageUrl == newItem.ImageUrl &&
                existingItem.Price == newItem.Price &&
                existingItem.PriceAfterSale == newItem.PriceAfterSale &&
                existingItem.Type == newItem.Type &&
                existingItem.UserId == newItem.UserId &&
                (existingItem.Color == newItem.Color || existingItem.Color == null && newItem.Color == null) &&
                (existingItem.Size == newItem.Size || existingItem.Size == null && newItem.Size == null) &&
                (existingItem.Text == newItem.Text || existingItem.Text == null && newItem.Text == null) &&
                (existingItem.Date == newItem.Date || existingItem.Date == null && newItem.Date == null)
            );
        }


    }
}
