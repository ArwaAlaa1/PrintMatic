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

       

    }



}
