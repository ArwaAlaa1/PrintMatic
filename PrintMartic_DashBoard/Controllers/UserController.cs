using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrintMartic_DashBoard.Helper.ViewModels;
using PrintMartic_DashBoard.Helper;
using PrintMartic_DashBoard.ViewModels;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace PrintMartic_DashBoard.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.Select(u => new UserViewModel()
            {
                Id = u.Id,
                Photo = u.Photo,
                UserName = u.UserName,
                DisplayName = u.DisplayName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                Location = u.Location,
                IsCompany = u.IsCompany,
                Roles = _userManager.GetRolesAsync(u).Result

            }).ToListAsync();

            return View(users);
        }

        public async Task<IActionResult> AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserFormViewModel addUser)
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

                        UserName = addUser.UserName,
                        DisplayName = addUser.DisplayName,
                        Email = addUser.Email,
                        PhoneNumber = addUser.PhoneNumber,
                        Location = addUser.Location,
                        IsCompany = addUser.IsCompany,
                        Photo = $"images/user/{addUser.Photo}"

                    };

                    await _userManager.CreateAsync(user, addUser.Password);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }

            }
            return View(addUser);
        }


        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var mappesuser = _mapper.Map<AppUser, UserFormViewModel>(user);
            return View(mappesuser);

        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserFormViewModel editUser)
        {

            try
            {


                //if (editUser.PhotoFile != null)
                //{
                //    editUser.Photo = DocumentSetting.UploadFile(editUser.PhotoFile, "user");
                //}



                var user = new AppUser()
                {
                    Id = editUser.Id,

                    UserName = editUser.UserName,
                    DisplayName = editUser.DisplayName,
                    Email = editUser.Email,
                    PhoneNumber = editUser.PhoneNumber,
                    Location = editUser.Location,
                    IsCompany = editUser.IsCompany,
                    //Photo = $"images/user/{editUser.Photo}"

                };

                await _userManager.UpdateAsync(user);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.InnerException.Message);
            }


            return View(editUser);
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var mappesuser = _mapper.Map<AppUser, UserFormViewModel>(user);
            return View(mappesuser);
        }

        public async Task<bool> AddRoleAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApplicationException($"User with ID '{userId}' not found.");
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<IActionResult> AddToRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var mappesuser = _mapper.Map<AppUser, UserFormViewModel>(user);
            var roles = await _roleManager.Roles.ToListAsync();
            mappesuser.Roles = roles;
            return View(mappesuser);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddToRole(UserFormViewModel user)
        {
            var role = await _roleManager.FindByIdAsync(user.RoleId);


            var result = await AddRoleAsync(user.Id, role.Name);

            if (!result)
            {
                return View("AddToRole");
            }
            return RedirectToAction("Index");
            //return Ok(new { message = $"User added to role '{role.Name}' successfully." });
        }


        public async Task<IActionResult> EditRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var roles = await _roleManager.Roles.ToListAsync();


            var viewmodel = new RoleUserViewModel()
            {
                Id = user.Id,

                DisplayName = user.DisplayName,

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
        public async Task<IActionResult> EditRole(RoleUserViewModel userRoleView)
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

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }
    }
}
