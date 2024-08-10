using Microsoft.AspNetCore.Identity;

namespace PrintMartic_DashBoard.ViewModels
{
    public class RolesViewModel
    {
        public IEnumerable<IdentityRole> Roles { get; set; }
    }
}
