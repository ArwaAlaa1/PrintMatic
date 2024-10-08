using PrintMatic.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Services
{
	public interface IOrderService
	{
		Task<Order> CreateOrderAsync(string CustomerEmail, string CartId,ShippingCost shippingCost, Address ShippingAddress);
		Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string CustomerEmail);
		Task<Order> GetOrderForUserAsync(int orderid,string CustomerEmail);
	}
}
