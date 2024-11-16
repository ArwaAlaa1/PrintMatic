using Microsoft.EntityFrameworkCore;
using PrintMatic.Core.Entities.Order;
using PrintMatic.Core.Repository.Contract;
using PrintMatic.Repository.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Repository.Repository
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {


        public OrderRepository(PrintMaticContext printMaticContext) : base(printMaticContext)
        {

        }



        public async Task<Order> GetOrderForUserAsync(int OrderId)
        {
            var order = await _context.Set<Order>().Where(O => O.Id == OrderId).Include(OI => OI.OrderItems).Include(o => o.ShippingCost).FirstAsync();
            return order;
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string Email)
        {
            var order = await _context.Set<Order>().Where(O => O.CustomerEmail == Email && O.IsDeleted == false).Include(o => o.ShippingCost).Include(o => o.OrderItems).ToListAsync();
            return order;
        }

        public async Task<Order> CancelOrderForUserAsync(int OrderId)
        {
            var order = await _context.Set<Order>().Where(O => O.Id == OrderId).Include(OI => OI.OrderItems).FirstAsync();
            order.Status = OrderStatus.Cancelled;
            order.IsActive = false;
            foreach (var item in order.OrderItems)
            {
                item.IsActive = false;
            }

            return order;

        }
        public async Task<Order> ReOrderForUserAsync(int OrderId)
        {
            var order = await _context.Set<Order>().Where(O => O.Id == OrderId).Include(OI => OI.OrderItems).FirstAsync();
            order.Status = OrderStatus.Pending;
            order.IsActive = true;
            foreach (var item in order.OrderItems)
            {
                item.IsActive = true;
            }
            return order;
        }

        public async Task<OrderItem> GetOrderItemAsync(int orderitemId)
        {
            var item = await _context.Set<OrderItem>().Where(oi => oi.Id == orderitemId).FirstAsync();
            return item;
        }

        public async Task<Order> DeleteOrderForUserAsync(int OrderId)
        {
            var order = await _context.Set<Order>().Where(O => O.Id == OrderId).Include(OI => OI.OrderItems).FirstAsync();

            order.IsDeleted = true;
            foreach (var item in order.OrderItems)
            {
                item.IsDeleted = true;
            }
            return order;
        }

        public IQueryable<Order> GetOrdersForSpecificCompanyAsync(string TraderId)
        {
            var orders = _context.Orders
         .Where(order => order.OrderItems.Any(item => item.TraderId == TraderId)).Include(oi => oi.OrderItems.Where(i => i.TraderId == TraderId))
         ;
            return orders;
        }

        public async Task<Order> GetOrderWithItemsAsync(int OrderId, string traderid)
        {
            var order = await _context.Orders
       .Where(order => order.Id == OrderId).Include(oi => oi.OrderItems.Where(i => i.TraderId == traderid))
       .FirstOrDefaultAsync();

            return order;
        }

    }
}
