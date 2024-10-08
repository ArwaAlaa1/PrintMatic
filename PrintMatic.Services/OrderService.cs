using PrintMatic.Core.Entities.Order;
using PrintMatic.Core.Repository.Contract;
using PrintMatic.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Services
{
	public class OrderService : IOrderService
	{
		private readonly ICartRepository _cartRepository;

		public OrderService(ICartRepository cartRepository)
		{
			_cartRepository = cartRepository;
		}
		public async Task<Order> CreateOrderAsync(string CustomerEmail, string CartId,ShippingCost shippingCost, Address ShippingAddress)
		{
		//	//1.Get Cart from cart repo
		//	var cart = await _cartRepository.GetCartAsync(CartId);

		//	//2 Get OrderItems 

		//	if (cart?.Items?.Count() > 0)
		//	{
		//		foreach (var item in cart.Items)
		//		{

		//			ProductOrderDetails product = new ProductOrderDetails(item.Id, item.PhotoURL, item.Type, item.ItemName, item.CategoryName, item.UserId, item.NormalMinDate, item.NormalMaxDate, item.UrgentMinDate, item.UrgentMaxDate, item.NormalPrice, item.PriceAfterSale, item.Description, item.Color, item.Size, item.Text, item.Date, item.Photos, item.FilePdf);
		//		}
		//	}

			throw new NotImplementedException();
		}

		public Task<Order> GetOrderForUserAsync(int orderid, string CustomerEmail)
		{
			throw new NotImplementedException();
		}

		public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string CustomerEmail)
		{
			throw new NotImplementedException();
		}
	}
}
