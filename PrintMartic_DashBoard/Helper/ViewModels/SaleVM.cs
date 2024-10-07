using Newtonsoft.Json.Serialization;
using Org.BouncyCastle.Asn1.Cmp;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PrintMartic_DashBoard.Helper.ViewModels
{
    public class SaleVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "تاريخ بدء الخصم مطلوب")]
        [DisplayName("تاريخ بدء الخصم")]
        public DateTime SaleStartDate { get; set; }
        [Required(ErrorMessage = "تاريخ انتهاء الخصم مطلوب")]
        [DisplayName("تاريخ انتهاء الخصم")]
        public DateTime SaleEndDate { get; set; }
        [DisplayName("نسبة الخصم")]
        [Required(ErrorMessage = "نسبة الخصم مطلوبه")]
        public int SaleDiscountPercentage { get; set; }
    }
}
