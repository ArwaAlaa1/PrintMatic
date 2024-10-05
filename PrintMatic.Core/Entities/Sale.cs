using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Entities
{
    public class Sale : BaseEntity
    {
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
