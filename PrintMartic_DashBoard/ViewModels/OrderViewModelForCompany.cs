using PrintMatic.Core.Entities.Order;

namespace PrintMartic_DashBoard.ViewModels
{
    public class OrderViewModelForCompany
    {
        public int Id { get; set; }
        //public string CustomerEmail { get; set; }
       public OrderStatus Status { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public List<OrderItemVM> OrderItems { get; set; }
        public int NumberItems { get; set; } 



    }
}
