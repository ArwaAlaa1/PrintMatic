using PrintMatic.Core.Entities.Identity;

namespace PrintMartic_DashBoard.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }
        public string Photo { get; set; }
        public bool IsCompany { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
