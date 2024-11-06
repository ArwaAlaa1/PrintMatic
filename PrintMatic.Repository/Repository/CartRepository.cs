using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Order;
using PrintMatic.Core.Repository.Contract;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PrintMatic.Repository.Repository
{
    public class CartRepository : ICartRepository
    {
       
        private readonly IDatabase _database;
		

		public CartRepository(IConnectionMultiplexer redis)
        {

            _database = redis.GetDatabase();
			
		}
        public async Task<bool> DeleteCartAsync(string cartId)
        {
            return await _database.KeyDeleteAsync(cartId);
        }

        public async Task<CustomerCart?> GetCartAsync(string cartId)
        {
            var cart= await _database.StringGetAsync(cartId);
            if (cart.IsNullOrEmpty)
            {
               return  new CustomerCart
                {
                    Id = cartId,
                    Items = new List<CartItems>()
                };

			}
            else
            {
                var customercart = JsonSerializer.Deserialize<CustomerCart>(cart);
				
                return customercart;
			}
			
        }

        public async Task<CustomerCart?> UpdateCartAsync(CustomerCart cart)
        {
            OrderSummary cost;
            if (cart?.orderSammery?.ShippingPrice!=null)
            {
                cost = new OrderSummary { ShippingPrice = cart.orderSammery.ShippingPrice };
            }
            else
            {
                cost = new OrderSummary { ShippingPrice = 0 };
            }
           
			var summary = new OrderSummary
			{
				TotalItems = cart.Items.Sum(item => item.Quantity),
				TotalPriceBeforeDiscount = cart.Items.Sum(item => item.Price * item.Quantity),
				ShippingPrice = cost.ShippingPrice, 
				TotalPriceAfterDiscount =cart.Items.Sum(item => item.TotalPrice),
				FinalTotal = cart.Items.Sum(item => item.TotalPrice) + cost.ShippingPrice,
				

			};
			cart.orderSammery = summary;
			var CreatedorUpdate = await _database.StringSetAsync(cart.Id, JsonSerializer.Serialize(cart), TimeSpan.FromDays(10));
            if (!CreatedorUpdate) return null;
            return await GetCartAsync(cart.Id);

        }
   
    
    }
}
