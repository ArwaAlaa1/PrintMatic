using Microsoft.AspNetCore.Identity;

namespace PrintMartic_DashBoard.ViewModels
{
    public class AddUserFormViewModel
    {
       
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }
        public string? Photo { get; set; }
        public IFormFile? PhotoFile { get; set; }
        public string Password { get; set; }

    }
}
