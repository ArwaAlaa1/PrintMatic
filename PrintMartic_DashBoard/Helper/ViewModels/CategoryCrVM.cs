using System.ComponentModel.DataAnnotations;

namespace PrintMartic_DashBoard.Helper.ViewModels
{
    public class CategoryCrVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="اسم القسم مطلوب")]
        public string Name { get; set; }
        [Required(ErrorMessage = "وصف القسم مطلوب")]
        public string Description { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل الصوره")]
        public IFormFile PhotoFile { get; set; }
        public string? PhotoURL { get; set; }
        public string? FilePath { get; set; }
    }
}
