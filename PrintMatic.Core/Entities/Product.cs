using PrintMatic.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Entities
{
    public class Product : BaseEntity
    {
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
        public bool Text { get; set; }
        public bool Date { get; set; }
        
        public bool Enter { get; set; }
        public decimal TotalPrice { get; set; }
        public virtual Category Category { get; set; }
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

		public virtual AppUser AppUser { get; set; }

		[ForeignKey(nameof(AppUser))]
		public string UserId { get; set; }

        public virtual ICollection<ProductPhotos> ProductPhotos { get; set; }
        public virtual ICollection<ProductSale> ProductSales { get; set; }
        public virtual ICollection<ProductColor> ProductColors { get; set; }
        public virtual ICollection<ProductSize> ProductSizes { get; set; }



    }
}
