using PrintMatic.Core.Entities;
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
            return cart.IsNullOrEmpty ? null: JsonSerializer.Deserialize<CustomerCart>( cart);
        }

        public async Task<CustomerCart?> UpdateCartAsync(CustomerCart cart)
        {
             var CreatedorUpdate = await _database.StringSetAsync(cart.Id, JsonSerializer.Serialize(cart), TimeSpan.FromDays(30));
            if (!CreatedorUpdate)  return null;
            return await GetCartAsync(cart.Id);
            
        }
    }
}
