using System.ComponentModel.DataAnnotations;

namespace PrintMartic_DashBoard.Helper.ViewModels
{
	public class CategoryVM
	{
		public int Id	{ get; set; }
        [Required(ErrorMessage = "اسم القسم مطلوب")]
        public string Name { get; set; }
        [Required(ErrorMessage = "وصف القسم مطلوب")]
        public string Description { get; set; }
        public IFormFile? PhotoFile { get; set; }
		public string? PhotoURL { get; set; }
        public string? FilePath { get; set; }



    }
}
