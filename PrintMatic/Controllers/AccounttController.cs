//using AutoMapper;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using PrintMatic.Core.Entities.Identity;
//using PrintMatic.DTOS;
//using PrintMatic.Errors;
//using PrintMatic.Extensions;
//using PrintMatic.Services;
//using System.Security.Claims;


//namespace PrintMatic.Controllers
//{
	
//        public class AccounttController : BaseApiController
//        {
//            private readonly UserManager<AppUser> _userManager;
//            private readonly SignInManager<AppUser> _signInManger;
//            private readonly ITokenService _tokenService;
//            //private readonly IMapper _mapper;

//            public AccounttController(
//                UserManager<AppUser> userManager,
//                SignInManager<AppUser> signInManger,
//                ITokenService tokenService
//                //IMapper mapper
//                )
//            {
//                _userManager = userManager;
//                _signInManger = signInManger;
//                _tokenService = tokenService;
//                //_mapper = mapper;
//            }

//            [HttpPost("login")]
//            public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
//            {
//                var user = await _userManager.FindByEmailAsync(loginDto.Email);
//                if (user == null) return Unauthorized(new ApiResponse(401));
//                var result = await _signInManger.CheckPasswordSignInAsync(user, loginDto.Password, false);
//                if (!result.Succeeded) return Unauthorized(new ApiResponse(401));


//                return Ok(new UserDto()
//                {
//                    DisplayName = user.DisplayName,
//                    Email = user.Email,
//                    Token = await _tokenService.CreateToken(user, _userManager)
//                });
//            }

//            [HttpPost("register")]
//            public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
//            {
//                if (CheckEmailExistAsync(registerDto.Email).Result.Value)
//                {
//                    return new BadRequestObjectResult(new ApiValidationErrorResponse() { Errors = new[] { "Email address is in use" } });
//                }
//                var user = new AppUser()
//                {
//                    DisplayName = registerDto.DisplayName,
//                    Email = registerDto.Email,
//                    UserName = registerDto.Email.Split('@')[0],
//                    PhoneNumber = registerDto.PhoneNumber,
//                    IsCompany = false
//                };

//                var result = await _userManager.CreateAsync(user, registerDto.Password);

//                if (!result.Succeeded) return BadRequest(new ApiResponse(400));

//                return Ok(new UserDto()
//                {
//                    DisplayName = user.DisplayName,
//                    Email = user.Email,
//                    Token = await _tokenService.CreateToken(user, _userManager)
//                });
//            }

//            [HttpGet("emailexists")]
//            public async Task<ActionResult<bool>> CheckEmailExistAsync(string email)
//            {
//                return await _userManager.FindByEmailAsync(email) != null;
//            }

//            [Authorize]
//            [HttpGet("GetCurrentUser")]
//            public async Task<ActionResult<UserDto>> GetCurrentUser()
//            {
//                var email = User.FindFirstValue(ClaimTypes.Email);
//                var user = await _userManager.FindByEmailAsync(email);
//                return Ok(new UserDto()
//                {
//                    Email = email,
//                    DisplayName = user.DisplayName,
//                    Token = await _tokenService.CreateToken(user, _userManager)
//                });
//            }

         
//        }
    
//}
