using PrintMatic.Core.Entities.Order;
using System.Globalization;

namespace PrintMatic.DTOS.OrderDTOS
{
    public class OrderReturnDto
    {
        public int Id { get; set; }

        public string OrderDate { get; set; }
        public string Status { get; set; } 
        public int Quantity { get; set; }
        //public int? ShippingCostId { get; set; }
        //public ShippingCost ShippingCost { get; set; }
        public decimal Total { get; set; }
        //public decimal GetTotal() => SubTotal + ShippingCost.Cost;
        public string OrderItemPhoto { get; set; } 
        //public string PaymentId { get; set; } = "";
    }
}
