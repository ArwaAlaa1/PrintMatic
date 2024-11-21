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
        public Task<IEnumerable<Order>> GetUserOrdersAsync(string Email);
        public Task<Order> GetOrderForUserAsync(int OrderId);
        public Task<Order> CancelOrderForUserAsync(int OrderId);
        public Task<Order> ReOrderForUserAsync(int OrderId);
        public Task<Order> DeleteOrderForUserAsync(int OrderId);
        public Task<OrderItem> GetOrderItemAsync(int orderitemId);
       

        //Specific Signatures for DashBoard as Trader
        public IQueryable<Order> GetOrdersForSpecificCompanyAsync(string TraderId);
        public Task<Order> GetOrderWithItemsForSpecificCompanyAsync(int OrderId,string traderid);

        public Task<Order> GetInvoiceForTraderAsync(int OrderId,string TraderId);
        //Specific Signatures for DashBoard as Admin

        public IQueryable<Order> GetOrdersForAdminAsync();
        public Task<Order> GetOrderForAdminAsync(int OrderId);
        public Task<Order> GetInvoiceForAdminAsync(int OrderId);
        public Task<OrderItem> CanceltOrderItemForAdminAsync(int ItemId);
    }
}
