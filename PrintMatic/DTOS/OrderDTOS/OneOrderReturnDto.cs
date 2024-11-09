using PrintMatic.Core.Entities.Order;

namespace PrintMatic.DTOS.OrderDTOS
{
    public class OneOrderReturnDto
    {
        public int OrderId { get; set; }

        //public string OrderDate { get; set; } 
        public string Status { get; set; }
        public int Quantity { get; set; }
       
       
        public ICollection<OrderItemInOrderReturnDto> OrderItems { get; set; }
        public AddressOrderReturnDto ShippingAddress { get; set; }
        public OrderSummary orderSummary { get; set; }
        public string PaymentId { get; set; } = "";

    }

}
