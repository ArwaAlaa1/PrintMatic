using PrintMatic.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Repository.Contract
{
    public interface IOrderRepository:IGenericRepository<Order> 
    {
        public Task<IEnumerable<Order>> GetUserOrders(string Email);
        public Task<Order> GetOrderForUser(int OrderId);
        public Task<Order> CancelOrderForUser(int OrderId);
        public Task<Order> ReOrderForUser(int OrderId);
    }
}
