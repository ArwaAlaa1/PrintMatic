using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrintMartic_DashBoard.Helper.ViewModels;
using PrintMartic_DashBoard.Helper;
using PrintMartic_DashBoard.ViewModels;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Identity;

namespace PrintMartic_DashBoard.Controllers
{
	public class UserController : Controller
	{
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
		{
            var users = await _userManager.Users.Select( u => new UserViewModel()
            {
                Id=u.Id,
                Photo=u.Photo,
                UserName=u.UserName,
                DisplayName=u.DisplayName,
                Email=u.Email,
                PhoneNumber=u.PhoneNumber,
                Location=u.Location,
                Roles=_userManager.GetRolesAsync(u).Result

            }).ToListAsync();

			return View(users);
		}

        public async Task<IActionResult> AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserFormViewModel addUser)
        {          
            addUser.Photo = addUser.PhotoFile.FileName;
           
            if (ModelState.IsValid)
            {
                try
                {
                    

                    if (addUser.PhotoFile != null)
                    {
                        addUser.Photo = DocumentSetting.UploadFile(addUser.PhotoFile, "user");
                    }
                  
                   

                    var user = new AppUser()
                    {
                   
                    UserName=addUser.UserName,
                    DisplayName=addUser.DisplayName,
                    Email=addUser.Email,
                    PhoneNumber=addUser.PhoneNumber,
                    Location=addUser.Location,
                    Photo= $"images/user/{addUser.Photo}"

                    };

                    await _userManager.CreateAsync(user);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }
               
            }
            return View(addUser);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var roles = await _roleManager.Roles.ToListAsync();
            var viewmodel = new RoleUserViewModel()
            {
                Id = user.Id,
                Name = user.DisplayName,
                Roles = roles.Select(roles => new RoleViewModel()
                {
                    Id = roles.Id,
                    RoleName = roles.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, roles.Name).Result

                }).ToList(),

            };
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RoleUserViewModel userRoleView)
        {
            var user = await _userManager.FindByIdAsync(userRoleView.Id.ToString());
            var rolesuser = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoleView.Roles)
            {
                if (rolesuser.Any(r => r == role.RoleName) && !role.IsSelected)
                {
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);

                }
                if (!rolesuser.Any(r => r == role.RoleName) && role.IsSelected)
                {
                    await _userManager.AddToRoleAsync(user, role.RoleName);

                }

            }
            return RedirectToAction(nameof(Index));
        }
    }
}
