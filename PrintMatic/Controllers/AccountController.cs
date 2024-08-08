using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.DTOS;


namespace PrintMatic.Controllers
{
	
	public class AccountController : BaseApiController
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;

		public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[HttpPost("register")]
		public async Task<ActionResult<UserDto>> SignUp(SignUPDto signUPDto)
		{

			var user = new AppUser()
			{
				DisplayName = signUPDto.DisplayName,
				Email = signUPDto.Email,
				PhoneNumber = signUPDto.PhoneNumber,
				UserName = signUPDto.Email.Split('@')[0],

			};
			var result =await _userManager.CreateAsync(user,signUPDto.Password);
			if (result.Succeeded is false)
				return BadRequest();

			return Ok(new UserDto()
			{
				DisplayName=user.DisplayName,
				Email=user.Email,
				Token="This Will Be Token"
			});
		}
    }
}
