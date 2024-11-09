using PrintMatic.Core.Entities.Identity;
using PrintMatic.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrintMatic.DTOS
{
    public class ProductDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int? NumOfPhotos { get; set; }
        public int NormalMinDate { get; set; } 
        public int NormalMaxDate { get; set; }
        public int UrgentMinDate { get; set; }
        public int UrgentMaxDate { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal UrgentPrice { get; set; }
        public bool Text { get; set; }
        public bool Date { get; set; }
        public UserSimpleDetails User { get; set; }
     
        public string CategoryName { get; set; }
        public List<string> Colors { get; set; }= new List<string>();
        public List<string> Sizes { get; set; } = new List<string>();
        public List<string> Photos { get; set; }= new List<string>();
        public decimal PriceAfterSale { get; set; }
        public float? AvgRating { get; set; }
    }
}
