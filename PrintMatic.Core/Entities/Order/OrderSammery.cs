//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PrintMatic.Core.Entities.Order
//{
//	public class OrderSammery
//	{
//        public OrderSammery(List<CartItems> items)
//        {
//			for (int i = 0; i < items.Count(); i++)
//			{
//				ItemCount += items[i].Quantity;
//				TotalPrice += items[i].Price;
//				TotalPriceAfterSale += items[i].PriceAfterSale;
//			}
//			FinalTotal = (decimal)(TotalPriceAfterSale > 0 ? TotalPriceAfterSale : TotalPrice);

//		}
        

//		public int ItemCount { get; set; }
//        public decimal TotalPrice { get; set; }
		
//		public decimal? TotalPriceAfterSale { get; set; }
//		public decimal FinalTotal { get; set; }
//	}
//}
