using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Order;

namespace PrintMartic_DashBoard.ViewModels
{
    public class OrderViewModelForCompany
    {
        public int Id { get; set; }
        //public string CustomerEmail { get; set; }
       public OrderStatus Status { get; set; }
        public OrderReady StatusReady { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public List<OrderItemVM> OrderItems { get; set; }
        public int NumberItems { get; set; }
       
        public Address ShippingAddress { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerUserName { get; set; }




    }
}
