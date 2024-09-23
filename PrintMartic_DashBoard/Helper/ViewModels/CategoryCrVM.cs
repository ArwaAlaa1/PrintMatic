namespace PrintMartic_DashBoard.Helper.ViewModels
{
    public class CategoryCrVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IFormFile PhotoFile { get; set; }
        public string? PhotoURL { get; set; }
        public string? FilePath { get; set; }
    }
}
