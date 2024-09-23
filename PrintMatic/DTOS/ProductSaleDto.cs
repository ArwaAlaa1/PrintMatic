using PrintMatic.Core.Entities;

namespace PrintMatic.DTOS
{
    public class ProductSaleDto
    {
        public int ProductId { get; set; }
        public int SaleId { get; set; }
        public decimal ProductAfterSale { get; set; }
    }
}
