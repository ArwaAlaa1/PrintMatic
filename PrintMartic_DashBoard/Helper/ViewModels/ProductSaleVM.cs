using PrintMatic.Core.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PrintMartic_DashBoard.Helper.ViewModels
{
    public class ProductSaleVM
    {
        public virtual Product? Product { get; set; }
        [UniquePRSale]
        [Required(ErrorMessage = " اسم المنتج مطلوب")]
        [DisplayName("اسم المنتج")]
        public int ProductId { get; set; }
        public virtual Sale? Sale { get; set; }
        [Required(ErrorMessage = " نسبة الخصم مطلوبه")]
        [DisplayName(" نسبة الخصم")]
        public int SaleId { get; set; }
        [DisplayName("السعر بعد الخصم")]
        public decimal? PriceAfterSale { get; set; } = 0.00m;
        public IEnumerable<Sale> Sales { get; set; } = new List<Sale>();
        public IEnumerable<Product> Products { get; set;} = new List<Product>();
    }
}
