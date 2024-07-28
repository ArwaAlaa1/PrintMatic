namespace PrintMartic_DashBoard.Helper.ViewModels
{
	public class CategoryVM
	{
		public int Id	{ get; set; }
		public string Name { get; set; }
		public IFormFile? PhotoFile { get; set; }
		public string? PhotoURL { get; set; }

	}
}
