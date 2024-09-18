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
using Microsoft.Identity.Client;

namespace PrintMartic_DashBoard.Controllers
{
    public class UserController : Controller
    {
        private readonly UserHelper userHelper;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public UserController(UserHelper userHelper,UserManager<AppUser> userManager, 
            RoleManager<IdentityRole> roleManager, IMapper mapper,IWebHostEnvironment environment)
        {
            this.userHelper = userHelper;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _environment = environment;
          
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.Select(u => new UserViewModel()
            {
                Id = u.Id,
                Photo = u.Photo,
                UserName = u.UserName,
                
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
               
                IsCompany = u.IsCompany,
                Roles = new List<string>() 
            }).Where(u=>u.IsCompany==false).ToListAsync();


            foreach (var user in users)
            {
                user.Roles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user.Id));
            }

            return View(users);
        }

        public async Task<IActionResult> AddUser()
        {

            UserFormViewModel user = new UserFormViewModel();
            var roles = await _roleManager.Roles.ToListAsync();
            user.Roles = roles;
            return View(user);
        }

        [HttpPost]

        public async Task<IActionResult> AddUser(UserFormViewModel addUser)
        {
            try
            {
                if (addUser.PhotoFile != null)
                {
                    addUser.Photo =  DocumentSetting.UploadFile(addUser.PhotoFile, "user",_environment.WebRootPath);
                }

                // Create a new AppUser object
                var user = new AppUser()
                {
                    UserName = addUser.UserName,
                   
                    Email = addUser.Email,
                    PhoneNumber = addUser.PhoneNumber,
                 
                    IsCompany = addUser.IsCompany,
                    Photo = $"images/user/{addUser.Photo}"
                };

                // Create the user in the system
                var createUserResult = await _userManager.CreateAsync(user, addUser.Password);

                if (createUserResult.Succeeded)
                {
                    // Add the role to the user after the user has been successfully created
                    var role = await _roleManager.FindByIdAsync(addUser.RoleId);

                    if (role != null)
                    {
                        var addRoleResult = await _userManager.AddToRoleAsync(user, role.Name);

                        if (!addRoleResult.Succeeded)
                        {
                            ModelState.AddModelError(string.Empty, "Failed to add role to the user.");
                            return View(addUser);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Role not found.");
                        return View(addUser);
                    }
                  
                     return RedirectToAction("Index");
                }
                else
                {
                    // If user creation failed, add the errors to the ModelState
                    foreach (var error in createUserResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(addUser);
        }

        public async Task<IActionResult> EditUser(string id)
        {


            var user = await userHelper.Edit(id);
            ViewData["ActionOne"] = "EditUser";
            return View(user);

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
                    
                    Email = editUser.Email,
                    PhoneNumber = editUser.PhoneNumber,
                    
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

     

       
        
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> EditVendor(string id)
        {
            var user2 = await userHelper.Edit(id);
            ViewData["ActionTwo"] = "EditVendor";
            var user = await _userManager.FindByIdAsync(id.ToString());
            var roles = await _roleManager.Roles.ToListAsync();


            var RoleVM = new RoleUserViewModel()
            {
                Id = user.Id,

               UserName= user.UserName,

                Roles = roles.Select(roles => new RoleViewModel()
                {
                    Id = roles.Id,
                    RoleName = roles.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, roles.Name).Result

                }).ToList(),

            };
            var UserRolemodel = new UserRoleEdit()
            {
                UserForm=user2,
                RoleForm=RoleVM

            };
           
            return View("Edit", UserRolemodel);

        }

        [HttpPost]
        public async Task<IActionResult> EditVendor(UserRoleEdit userRoleEdit)
        {
            try
            {
                // Fetch the existing user from the UserManager
                var user = await _userManager.FindByIdAsync(userRoleEdit.UserForm.Id.ToString());

                if (user == null)
                {
                    return NotFound();
                }

                // Update roles
                var rolesuser = await _userManager.GetRolesAsync(user);
                foreach (var role in userRoleEdit.RoleForm.Roles)
                {
                    if (rolesuser.Contains(role.RoleName) && !role.IsSelected)
                    {
                        await _userManager.RemoveFromRoleAsync(user, role.RoleName);
                    }
                    else if (!rolesuser.Contains(role.RoleName) && role.IsSelected)
                    {
                        await _userManager.AddToRoleAsync(user, role.RoleName);
                    }
                }


                user.UserName = userRoleEdit.UserForm.UserName;
                
                user.Email = userRoleEdit.UserForm.Email;
                user.PhoneNumber = userRoleEdit.UserForm.PhoneNumber;
               
                user.IsCompany = userRoleEdit.UserForm.IsCompany;
                user.Photo = $"images/user/{userRoleEdit.UserForm.Photo}";


                await _userManager.UpdateAsync(user);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log exception or handle it accordingly
                return StatusCode(500, "Internal server error");
            }
        }

       
        public async Task<IActionResult> GetVendors(string Name)
        {
            var allUsers = await _userManager.Users.Select(u => new UserViewModel()
            {
                Id = u.Id,
                Photo = u.Photo,
                UserName = u.UserName,
               
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
               
                IsCompany = u.IsCompany,
                Roles = new List<string>()
            }).ToListAsync();
            foreach (var user in allUsers)
            {
                user.Roles = await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user.Id));
            }


            var usersWithRole = allUsers.Where(u => u.Roles.Contains(Name)).ToList();
            var users = usersWithRole.Where(u => u.IsCompany == true);
            return View(users);
        }
    }
}
