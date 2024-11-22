using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Entities.Order
{
	public class Order:BaseEntity
	{
		public Order()
		{

		}
		public Order(string customerEmail, Address shippingAddress, decimal subTotal, ICollection<OrderItem> orderItems,ShippingCost shippingCost)
		{
			CustomerEmail = customerEmail;
			ShippingAddress = shippingAddress;
			SubTotal = subTotal;
			OrderItems = orderItems;
			ShippingCost = shippingCost;

        }

		public string CustomerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }=DateTimeOffset.Now;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public OrderReady StatusReady { get; set; } =OrderReady.NotReady;
        public Address ShippingAddress { get; set; }
		public int? ShippingCostId { get; set; }
		public ShippingCost ShippingCost { get; set; }
		public decimal SubTotal { get; set; }
        public decimal GetTotal() => SubTotal+ShippingCost.Cost;
        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public string PaymentId { get; set; } = "";

    }
}
