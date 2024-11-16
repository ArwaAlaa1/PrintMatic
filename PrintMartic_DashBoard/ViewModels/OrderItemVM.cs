using PrintMatic.Core.Entities.Order;

namespace PrintMartic_DashBoard.ViewModels
{
    public class OrderItemVM
    {
        public int Id { get; set; }
        public ProductOrderDetails ProductItem { get; set; }
       
        public OrderItemStatus OrderItemStatus { get; set; } 
        public decimal TotalPrice { get; set; }

        public int Quantity { get; set; }
    }
}
