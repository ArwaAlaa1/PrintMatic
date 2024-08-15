using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrintMartic_DashBoard.ViewModels;
using PrintMatic.Core.Entities.Identity;

namespace PrintMartic_DashBoard.Helper
{
    public class UserHelper
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UserHelper(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }
        public async Task<AppUser> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user;
        }
        public async Task<UserFormViewModel> Edit(string id)
        {
            var user = await GetUserByIdAsync(id);
            var mappesuser = _mapper.Map<AppUser, UserFormViewModel>(user);
            return mappesuser;

        }
    }
}
