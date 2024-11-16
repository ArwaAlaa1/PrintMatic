using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Entities.Order
{
	public class OrderItem:BaseEntity
	{

        public OrderItem()
        {
            
        }

		public OrderItem(ProductOrderDetails productItem,/* decimal price,*/ int quantity,string traderId)
		{
			ProductItem = productItem;
			//Price = price;
			Quantity = quantity;
			TraderId = traderId;

        }

		public ProductOrderDetails ProductItem{ get; set; }
        public int OrderId { get; set; }
        public OrderItemStatus OrderItemStatus { get; set; } = OrderItemStatus.Pending;
		private decimal totalprice;

		public decimal TotalPrice
		{
			get { return totalprice; }
			set { totalprice = value; }
		}

		//public decimal TotalPrice { get; set; }
		//public decimal TotalPrice => (decimal)(ProductItem.PriceAfterSale == 0 ? ProductItem.Price * Quantity : ProductItem.PriceAfterSale * Quantity);

		public string TraderId { get; set; }
		public int Quantity { get; set; }
	}
}
