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

		public OrderItem(ProductOrderDetails productItem, decimal price, int quantity)
		{
			ProductItem = productItem;
			Price = price;
			Quantity = quantity;
		}

		public ProductOrderDetails ProductItem{ get; set; }
        public decimal Price { get; set; }
		public int Quantity { get; set; }
	}
}
