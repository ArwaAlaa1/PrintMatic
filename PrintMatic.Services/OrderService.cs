using PrintMatic.Core;
using PrintMatic.Core.Entities;
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
        private readonly IUnitOfWork _unitOfWork;
      

        public OrderService(ICartRepository cartRepository, IUnitOfWork unitOfWork)
		{
			_cartRepository = cartRepository;
           _unitOfWork = unitOfWork;
          
        }
		public async Task<Order?> CreateOrderAsync(string CustomerEmail, string CartId,int shippingCostId, Address ShippingAddress)
		{
			//1.Get Cart from cart repo
			var cart = await _cartRepository.GetCartAsync(CartId);

			//2 Get OrderItems  fro product 
			var OrderItems=new List<OrderItem>();	

			if (cart?.Items?.Count() > 0)
			{
				foreach (var item in cart.Items)
				{
                    ProductOrderDetails productDetails = new ProductOrderDetails();
					var Photos = string.Empty;
                    if (item?.Photos?.Count() > 0)
					{
                       Photos = ImageService.ConvertUrlsToJson(item.Photos);
                    }
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);
                    if (item.Type == "عادى")
                    {
                        productDetails = new ProductOrderDetails(item.ProductId, item.ImageUrl, ItemType.Normal, product.Name, product.NormalPrice, item.PriceAfterSale, item.Color, item.Size, item.Text, item.Date, Photos, item.FilePdf); ;

                    }
                    else
                    {
                        productDetails = new ProductOrderDetails(item.ProductId, item.ImageUrl, ItemType.Urgent, product.Name, product.UrgentPrice, item.PriceAfterSale, item.Color, item.Size, item.Text, item.Date, Photos, item.FilePdf); ;
                    }
                    var orderitem = new OrderItem(productDetails, item.Quantity);
					orderitem.TotalPrice = (decimal)(productDetails.PriceAfterSale == 0 ? productDetails.Price * item.Quantity : productDetails.PriceAfterSale * item.Quantity);

                    OrderItems.Add(orderitem);

                }
            }

			//calc subtotal

			var TotalPrice = OrderItems.Sum(OI => OI.TotalPrice);
			//get shippingcost
			var ShippingCost = await _unitOfWork.Repository<ShippingCost>().GetByIdAsync(shippingCostId);
			//createorder

			var order=new Order(CustomerEmail,ShippingAddress, TotalPrice,OrderItems, ShippingCost);
			//save to db
			 _unitOfWork.Repository<Order>().Add(order);
			var rows=await _unitOfWork.Complet();
			if (rows<=0)
			     return null;

			return order;


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
