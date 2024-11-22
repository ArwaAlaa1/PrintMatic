using PrintMatic.Core.Entities.Order;

namespace PrintMatic.DTOS.OrderDTOS
{
    public class DeliveryOrderDto
    {
        public int OrderNum { get; set; }
        public string OrderDate { get; set; } 
        public string OrderStatus { get; set; }
        public string TraderStatus { get; set; }

        public AddressDeliveryDto   ShippingAddress { get; set; }
        public int ShippingCost { get; set; }
        
        public decimal SubTotal { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
     
        public List<OrderItemInDelivery> OrderItems { get; set; } 
        public string PaymentId { get; set; } = "";
    }
}
