using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Entities.Order
{
	public class OrderSummary
	{
		
			public int TotalItems { get; set; }
			public decimal TotalPriceBeforeDiscount { get; set; }
		    public decimal ShippingPrice { get; set; } = 5.0M;
			public decimal TotalPriceAfterDiscount { get; set; }
		

		public decimal FinalTotal { get; set; }
	}
}
