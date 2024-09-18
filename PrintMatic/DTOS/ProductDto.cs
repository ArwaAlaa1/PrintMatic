using PrintMatic.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace PrintMatic.DTOS
{
    public class ProductDto
    {

        public int Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
        //public decimal Rating { get; set; }
        public decimal TotalPrice { get; set; }
        public ProductSaleDto productSale { get; set; }
        public IEnumerable<ProductPhotoDto> photos { get; set; }

    }
}
