using PrintMatic.Core.Entities.Order;

namespace PrintMatic.DTOS.OrderDTOS
{
    public class OrderItemInDelivery
    {
        public int ItemId { get; set; }
        public string Photo { get; set; }
       
        public string Name { get; set; }
     
        public string OrderItemStatus { get; set; }
        public decimal TotalPrice { get; set; }

        

        //public string CompanyName { get; set; }
        //public string CompanyLocation { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}
