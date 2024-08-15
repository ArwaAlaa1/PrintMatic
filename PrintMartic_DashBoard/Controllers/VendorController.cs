using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrintMartic_DashBoard.ViewModels;
using PrintMatic.Core.Entities.Identity;

namespace PrintMartic_DashBoard.Controllers
{
    public class VendorController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public VendorController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> GetVendors(string Name)
        {
            var allUsers = await _userManager.Users.Select(u => new UserViewModel()
            {
                Id = u.Id,
                Photo = u.Photo,
                UserName = u.UserName,
                DisplayName = u.DisplayName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Location = u.Location,
                IsCompany = u.IsCompany,
                Roles = new List<string>()
            }).ToListAsync();
            foreach (var user in allUsers)
            {
                user.Roles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user.Id));
            }


            var usersWithRole = allUsers.Where(u => u.Roles.Contains(Name)).ToList();

            return View(usersWithRole);
        }


    }
}
