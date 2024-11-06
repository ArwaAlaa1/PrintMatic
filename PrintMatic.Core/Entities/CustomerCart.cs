using PrintMatic.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Entities
{
    public class CustomerCart
    {
        public CustomerCart()
        {
            
        }
        public CustomerCart(string id)
        {
            Id = id;
            Items = new List<CartItems>();
            //orderSammery=new OrderSammery(Items);
        }

        public string Id { get; set; }
        public List<CartItems> Items { get; set; }
        public bool IsProductUnique(CartItems newItem)
        {
            return !Items.Any(existingItem =>
                existingItem.ProductId == newItem.ProductId &&
                existingItem.ProductName == newItem.ProductName &&
                existingItem.ImageUrl == newItem.ImageUrl &&
                existingItem.Price == newItem.Price &&
                existingItem.PriceAfterSale == newItem.PriceAfterSale &&
                existingItem.Type == newItem.Type &&
                existingItem.UserId == newItem.UserId &&
                (existingItem.Color == newItem.Color || (existingItem.Color == null && newItem.Color == null)) &&
                (existingItem.Size == newItem.Size || (existingItem.Size == null && newItem.Size == null)) &&
                (existingItem.Text == newItem.Text || (existingItem.Text == null && newItem.Text == null)) &&
                (existingItem.Date == newItem.Date || (existingItem.Date == null && newItem.Date == null))
            );
        }
        public CartItems? FindMatchingProduct(CartItems newItem)
        {
            return Items.FirstOrDefault(existingItem =>
                existingItem.ProductId == newItem.ProductId &&
                existingItem.ProductName == newItem.ProductName &&
                existingItem.ImageUrl == newItem.ImageUrl &&
                existingItem.Price == newItem.Price &&
                existingItem.PriceAfterSale == newItem.PriceAfterSale &&
                existingItem.Type == newItem.Type &&
                existingItem.UserId == newItem.UserId &&
                (existingItem.Color == newItem.Color || (existingItem.Color == null && newItem.Color == null)) &&
                (existingItem.Size == newItem.Size || (existingItem.Size == null && newItem.Size == null)) &&
                (existingItem.Text == newItem.Text || (existingItem.Text == null && newItem.Text == null)) &&
                (existingItem.Date == newItem.Date || (existingItem.Date == null && newItem.Date == null))
            );
        }

        public OrderSummary  orderSammery { get; set; }

    }
}
