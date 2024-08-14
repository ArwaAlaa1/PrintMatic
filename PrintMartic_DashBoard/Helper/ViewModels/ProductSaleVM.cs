using PrintMatic.Core.Entities;

namespace PrintMartic_DashBoard.Helper.ViewModels
{
    public class ProductSaleVM
    {
        public virtual Product? Product { get; set; }
        [UniquePRSale]
        public int ProductId { get; set; }
        public virtual Sale? Sale { get; set; }
        public int SaleId { get; set; }
        public decimal PriceAfterSale { get; set; }
        public IEnumerable<Sale> Sales { get; set; } = new List<Sale>();
        public IEnumerable<Product> Products { get; set;} = new List<Product>();
    }
}
