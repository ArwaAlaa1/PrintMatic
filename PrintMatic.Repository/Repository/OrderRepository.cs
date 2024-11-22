using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
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
            var order = await _context.Set<Order>().Where(O => O.Id == OrderId).Include(OI => OI.OrderItems).FirstOrDefaultAsync();
            order.Status = OrderStatus.Cancelled;
            //order.IsActive = false;
            foreach (var item in order.OrderItems)
            {
                item.OrderItemStatus = OrderItemStatus.Cancelled;
            }

            return order;

        }
        public async Task<Order> ReOrderForUserAsync(int OrderId)
        {
            var order = await _context.Set<Order>().Where(O => O.Id == OrderId).Include(OI => OI.OrderItems).FirstAsync();
            order.Status = OrderStatus.Pending;
            //order.IsActive = true;
            foreach (var item in order.OrderItems)
            {
                //item.IsActive = true;
                item.OrderItemStatus = OrderItemStatus.Pending;
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

        //****************Specific Signatures for DashBoard as Trader***************
        //Get Orders For Specific Company
        public IQueryable<Order> GetOrdersForSpecificCompanyAsync(string TraderId, OrderItemStatus status)
        {
            var orders = _context.Orders
         .Where(order => order.OrderItems.Any(item => item.TraderId == TraderId && item.OrderItemStatus == status))
         .Include(oi => oi.OrderItems.Where(i => i.TraderId == TraderId )).Include(o=>o.ShippingAddress);
            return orders;
        }

        public IQueryable<Order> GetOrdersForSpecificCompanyAsync(string TraderId)
        {
            var orders = _context.Orders
         .Where(order => order.OrderItems.Any(item => item.TraderId == TraderId))
         .Include(oi => oi.OrderItems.Where(i => i.TraderId == TraderId)).Include(o => o.ShippingAddress);
            return orders;
        }

        //Get Order With Items Of Specific Trader
        public async Task<Order> GetOrderWithItemsForSpecificCompanyAsync(int OrderId, string traderid)
        {
            var order = await _context.Orders
       .Where(order => order.Id == OrderId)
       .Include(oi => oi.OrderItems.Where(i => i.TraderId == traderid))
       .FirstOrDefaultAsync();

            return order;
        }

        public  async Task<Order> GetInvoiceForTraderAsync(int OrderId,string TraderId)
        {
            var order = await _context.Orders
 .Where(order => order.Id == OrderId)
 .Include(oi => oi.OrderItems.Where(i => i.TraderId == TraderId))
 .Include(o=>o.ShippingAddress).Include(o=>o.ShippingCost)
 .FirstOrDefaultAsync();

            return order;
        }

        //************Specific Signatures for DashBoard as Admin***************

        //Get All Orders
        public IQueryable<Order> GetOrdersForAdminAsync()
        {
            var orders = _context.Orders.Include(o=>o.OrderItems);
            return orders;
        }

        //Get Order With OrderItems Details
        public async Task<Order> GetOrderForAdminAsync(int OrderId)
        {

            var order = await _context.Orders
               .Where(order => order.Id == OrderId)
               .Include(oi => oi.OrderItems).FirstOrDefaultAsync();

            return order;
        }

        //GetInvoice
        public async Task<Order> GetInvoiceForAdminAsync(int OrderId)
        {
            var order = await _context.Orders
       .Where(order => order.Id == OrderId)
       .Include(oi => oi.OrderItems).Include(oi => oi.ShippingAddress)
               .Include(oi => oi.ShippingCost).FirstOrDefaultAsync();
     
            return order;
        }

        public async Task<OrderItem> CanceltOrderItemForAdminAsync(int ItemId)
        {
            var orderItem = await _context.OrderItems
           .Where(oi => oi.Id == ItemId).FirstOrDefaultAsync();
            orderItem.OrderItemStatus = OrderItemStatus.Cancelled;
            
            return orderItem;
        }

        public async Task<IQueryable<Order>> GetReadyOrders()
        {
            var orders =_context.Orders
                .Where(oi => oi.OrderItems.All(oi => oi.OrderItemStatus == OrderItemStatus.Ready))
                .AsQueryable();
            return orders;
        }
    }
}
