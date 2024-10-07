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
        [Required(ErrorMessage = "اسم المنتج مطلوب")]
        public string Name { get; set; }
        [Required(ErrorMessage = "وصف المنتج مطلوب")]
        public string Description { get; set; }
        
        public int? NumOfPhotos { get; set; }
        [Required(ErrorMessage = "الحد الأدنى للتجهيز مطلوب")]
        public int NormalMinDate { get; set; } //int
        [Required(ErrorMessage = "الحد الأقصى للتجهيز مطلوب")]
        public int NormalMaxDate { get; set; }
        [Required(ErrorMessage = "الحد الأدنى للتجهيز المستعجل مطلوب")]
        public int UrgentMinDate { get; set; }
        [Required(ErrorMessage = "الحد الأقصى للتجهيز المستعجل مطلوب")]
        public int UrgentMaxDate { get; set; }
        [Required(ErrorMessage = "السعر العادى مطلوب")]
        public decimal NormalPrice { get; set; }
        [Required(ErrorMessage = "السعر المستعجل مطلوب")]
        public decimal UrgentPrice { get; set; }

        public bool Text { get; set; }
        public bool Date { get; set; }
        public bool Enter { get; set; }
        public decimal TotalPrice { get; set; }

        public virtual Category? Category { get; set; }
        [Required(ErrorMessage = "اسم القسم مطلوب")]
        public int CategoryId { get; set; }
        public IEnumerable<Category> Categories { get; set; }= new List<Category>();
        public virtual AppUser? AppUser { get; set; }
        [Required(ErrorMessage = "اسم البائع مطلوب")]
        public string UserId { get; set; }
        public IEnumerable<AppUser> Users { get; set; } = new List<AppUser>();
        public string? ColorJson { get; set; }
        public string? SizeJson { get; set; }
        public List<ProductColor>? Colors { get; set; } = new List<ProductColor>();
        public List<ProductSize>? Sizes { get; set; }= new List<ProductSize>();
       
    }
}
