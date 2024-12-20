﻿using PrintMatic.Core.Entities.Order;

namespace PrintMatic.DTOS.OrderDTOS
{
    public class OrderItemInOrderReturnDto
    {
        
        public int ProductId { get; set; }
        public string Photo { get; set; }
        
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal? PriceAfterSale { get; set; }

      
    }
}
