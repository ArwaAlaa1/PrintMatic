using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Repository.Contract;
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
        public decimal PriceAfterSale {  get; set; }
        public string FilePath { get; set; }
        public float? AvgRating { get; set; }

       


        public static async Task<ProductDto> GetProducts(ProductDto product,IEnumerable<ProductSale> sales,IEnumerable<ProductPhotos> photos,IEnumerable<Review> reviews)
        {
           
                if (sales.ToList().Count > 0)
                {
                    var list = new List<ProductSale>();
                    foreach (var sale in sales)
                    {
                        if (sale.Sale.SaleEndDate > DateTime.UtcNow)
                        {
                            list.Add(sale);
                        }
                    }
                    var item = list.FirstOrDefault();
                    if (item != null)
                    {
                        product.PriceAfterSale = item.PriceAfterSale;
                    }
                }
                if (photos.ToList().Count > 0)
                {
                    var pitem = photos.FirstOrDefault();
                    if (pitem != null)
                    {
                        product.FilePath = pitem.FilePath;
                    }
                }
                float? Rating = 0f;
                foreach (var review in reviews)
                {
                    if (review != null)
                    {
                        Rating += review.Rating;
                    }

                }
                product.AvgRating = Rating / 5f;
            
            return product;
            

        }

    }

    

}
