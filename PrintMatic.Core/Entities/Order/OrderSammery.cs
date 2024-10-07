using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Entities.Order
{
	public class OrderSammery
	{
        public OrderSammery()
        {
            
        }
        

		public int ItemCount { get; set; }
        public decimal TotalPrice { get; set; }
		
		public decimal? TotalPriceAfterSale { get; set; }
		public decimal FinalTotal { get; set; }
	}
}
