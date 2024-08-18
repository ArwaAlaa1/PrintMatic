using PrintMatic.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using PrintMatic.Core.Entities.Identity;

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
        public bool Enter { get; set; }
        public decimal TotalPrice { get; set; }

        public virtual Category? Category { get; set; }
        
        public int CategoryId { get; set; }
        public IEnumerable<Category> Categories { get; set; }= new List<Category>();

		public virtual AppUser? AppUser { get; set; }
		public string UserId { get; set; }
        public IEnumerable<AppUser> Users { get; set; } = new List<AppUser>();

	}
}
