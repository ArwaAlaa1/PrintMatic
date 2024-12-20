﻿using PrintMatic.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Services
{
	public interface IOrderService
	{
		Task<Order?> CreateOrderAsync(string CustomerEmail, string CartId,int shippingCostId, Address ShippingAddress);
		Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string Email);
		Task<Order> GetOrderForUserAsync(int orderid);
        Task<Order> CancelOrderForUserAsync(int orderid);
        Task<Order> ReOrderForUserAsync(int orderid);
        Task<OrderItem> GetOrderItemForOrder(int orderItemId);
        Task<Order> DeleteOrderForUserAsync(int orderid);
    }
}
