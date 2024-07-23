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

        public DateTime NormalMinDate { get; set; }
        public DateTime NormalMaxDate { get; set; }
        public DateTime UrgentMinDate { get; set; }
        public DateTime UrgentMaxDate { get; set; }

        public decimal NormalPrice { get; set; }
        public decimal UrgentPrice { get;set; }

        public bool Color { get; set; }
        public bool Text { get; set; }
        public bool Date { get; set; }
        
        public Category Category { get; set; }
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }

    }
}
