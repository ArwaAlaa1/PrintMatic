using Microsoft.AspNetCore.Identity;

namespace PrintMartic_DashBoard.ViewModels
{
    public class UserFormViewModel
    {
        public string? Id { get; set; }

        public string UserName { get; set; }
       
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }
        public string? Photo { get; set; }
        public IFormFile? PhotoFile { get; set; }
        public string Password { get; set; }
        public bool IsCompany { get; set; }
        public string? RoleId { get; set; }
        public IEnumerable<IdentityRole>? Roles { get; set; }

    }
}
