using PrintMatic.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PrintMartic_DashBoard.Helper.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public string Name { get; set; }
        public string Description { get; set; }

        public int? NumOfPhotos { get; set; }

        public int NormalMinDate { get; set; } //int
        public int NormalMaxDate { get; set; }
        public int UrgentMinDate { get; set; }
        public int UrgentMaxDate { get; set; }

        public decimal NormalPrice { get; set; }
        public decimal UrgentPrice { get; set; }

        public bool Color { get; set; }
        public bool Text { get; set; }
        public bool Date { get; set; }

        public virtual Category? Category { get; set; }
        
        public int CategoryId { get; set; }
    }
}
