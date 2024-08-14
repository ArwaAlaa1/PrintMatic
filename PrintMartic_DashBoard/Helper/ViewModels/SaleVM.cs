using System.ComponentModel;

namespace PrintMartic_DashBoard.Helper.ViewModels
{
    public class SaleVM
    {
        public int Id { get; set; }
   
        public DateTime SaleStartDate { get; set; }
        public DateTime SaleEndDate { get; set; }
        [DisplayName("Discount")]
        public int SaleDiscountPercentage { get; set; }
    }
}
