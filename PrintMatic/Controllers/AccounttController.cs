using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PrintMatic.Core;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.Core.Repository.Contract;
using PrintMatic.Core.Services;
using PrintMatic.DTOS.IdentityDTOS;
using PrintMatic.Errors;
using PrintMatic.Extensions;
using PrintMatic.Helper;
using PrintMatic.Services;
using System.Security.Claims;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;


namespace PrintMatic.Controllers
{

    public class AccounttController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManger;
        private readonly ITokenService _tokenService;
		private readonly IUserService _userService;
		private readonly IEmailService _emailService;
		private readonly CodeVerificationService _verificationService;
		private readonly SignInManager<AppUser> _signInManager;
        private readonly IUnitOfWork<Address> _unitOfWork;
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _apiBaseUrl;
        public readonly string _imagepath;
        public AccounttController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManger,
            ITokenService tokenService,
            IUserService userService,
            IEmailService emailService,
            CodeVerificationService verificationService,
			SignInManager<AppUser> signInManager,
            IUnitOfWork<Address> unitOfWork,
            IAddressRepository addressRepository,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment,
            IConfiguration configuration
            )

        {
            _userManager = userManager;
            _signInManger = signInManger;
            _tokenService = tokenService;
			_userService = userService;
			_emailService = emailService;
			_verificationService = verificationService;
			_signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _addressRepository = addressRepository;
          
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _apiBaseUrl = configuration["ApiBaseUrl"];
            _imagepath = $"{_webHostEnvironment.WebRootPath}";

        }


        //Login EndPoint Domain/Api/Account/login
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.EmailOrUserName)??
                                await _userManager.FindByNameAsync(loginDto.EmailOrUserName); 
            if (user == null) return Unauthorized(new ApiResponse(401));

			var result = await _signInManger.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
				return Unauthorized(new
				{
					StatusCode = 401,
					Message = "اسم المستخدم أو كلمة المرور غير صحيحة"
				});

		

            return Ok(new UserDto()
            {
                UserName=user.UserName,
                Email = user.Email,
                Token = await _tokenService.CreateToken(user, _userManager)
            });
        }


		//Register EndPoint Domain/Api/Account/register
		[HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
			var emailexsist = await _userManager.FindByEmailAsync(registerDto.Email);
            if (emailexsist == null)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse() { Errors = new[] { "البريد الإلكتروني مستخدم بالفعل" } });
            }
            var user = new AppUser()
            {
                
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber,
                IsCompany = false
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

			if (!result.Succeeded)
			{
				
				var errors = result.Errors.Select(e => _userService.GetCustomErrorsMessage(e)).ToArray();
				
				return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = errors });
			}

		
            return Ok(new UserDto()
            {
                UserName=user.UserName,
                Email = user.Email,
                Token = await _tokenService.CreateToken(user, _userManager)
            });
        }

      

		//ForgetPassword EndPoint Domain/Api/Account/forgetpassword
		[Authorize]
        [HttpPost("forgetpassword")]
        public async Task<ActionResult> ForgetPassword(ForgetPasswordDTO forgetPasswordDto)
        {
			
			var user = await _userManager.FindByEmailAsync(forgetPasswordDto.Email);
			if (user == null)
			{
				
				return BadRequest("هذا البريد الالكترونى غير موجود");
			}

			var OTP = new Random().Next(1000, 9999).ToString();
           
            await _verificationService.SaveVerificationCodeAsync(user.Id, OTP);

			await _emailService.SendEmailAsync(user.Email, " رمز التحقق من giftly", OTP);

			return Ok(new ApiResponse(200,"!تم إرسال الكود بنجاح "));
		}

		//VerifyCode EndPoint Domain/Api/Account/VerifyCode
		[Authorize]
        [HttpPost("VerifyCode")]
        public async Task<ActionResult> VerifyCode(VerifyCodeDto verifyCodeDto)
        {
			var email = User.FindFirstValue(ClaimTypes.Email);
			if (string.IsNullOrEmpty(email))
			     return Unauthorized("المستخدم غير مسجل الدخول");
			
			var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("المستخدم غير موجود"); 

			
            var otp = await _verificationService.GetVerificationCode(user.Id);
			
			if (otp == null)
                return BadRequest("هذا الكود غير صالح");
           
            else if(otp != verifyCodeDto.Code)
                return BadRequest("هذا الكود غير صحيح");
			else
				return Ok(new { Message = "تم تأكيد الكود بنجاح" });




		}

		//ResendCode EndPoint Domain/Api/Account/ResendCode
		[Authorize, HttpGet("ResendCode")]
        public async Task<ActionResult> ResendCode()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                return BadRequest(new { Message = "المستخدم غير مسجل الدخول" });
           //var user1 = await _userManager.GetUserAsync(User);

            var user2 = await _userManager.FindByEmailAsync(email);
            if(user2 == null)
				return BadRequest("المستخدم غير موجود");

			var OTP = new Random().Next(1000, 9999).ToString();

			await _verificationService.SaveVerificationCodeAsync(user2.Id, OTP);

			await _emailService.SendEmailAsync(user2.Email, " رمز التحقق من giftly", OTP);

			return Ok(new ApiResponse(200, "!تم إرسال الكود بنجاح "));
		}

        //ResetPassword EndPoint Domain/Api/Account/ResetPassword
        [Authorize]
        [HttpPost("ResetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            if (user == null)
				return BadRequest( new 
                { 
                Message="المستخدم غير موجود"
                });
			var token = await _userManager.GeneratePasswordResetTokenAsync(user);
			if (string.IsNullOrEmpty(token))
			{
				return BadRequest("فشل في إنشاء رمز إعادة تعيين كلمة المرور");
			}
			var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordDto.NewPassword);
			if (!result.Succeeded)
			{
				// Collect all errors and return them as a response
				var errors = result.Errors.Select(e => _userService.GetCustomErrorsMessage(e)).ToArray();
				
				return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = errors });
			}
			

			return Ok(new
			{
				StatusCode=200,
				Message="!تم تعديل كلمه السر بنجاح "
			});
		}

        //ChangePassword EndPoint Domain/Api/Account/ChangePassword
        [Authorize]
		[HttpPost("ChangePassword")]
		public async Task<ActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
		{

			var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
			if (user == null)
				return BadRequest(new
				{
					Message = "المستخدم غير موجود"
				});
			var checkOldpass =  _userManager.CheckPasswordAsync(user, changePasswordDto.OldPassword);
			
            if (!checkOldpass.Result)
			{
				return BadRequest(new
				{
					Message = "كلمة السر القديمه خطأ "
				});
			}
			if (changePasswordDto.OldPassword == changePasswordDto.NewPassword)
			{
				return BadRequest(new
				{
					Message = "كلمه السر الجديد مثل القديمه"
				});
			}
			var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);
			if (!result.Succeeded)
			{
				// Collect all errors and return them as a response
				var errors = result.Errors.Select(e => _userService.GetCustomErrorsMessage(e)).ToArray();

				return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = errors });
			}
			else
			{
                await _signInManager.RefreshSignInAsync(user);	
				
			return Ok(new
			{
               
            Message = "!تم تغيير كلمه السر بنجاح "
			});
            }


		
		}

        //Logout EndPoint Domain/Api/Account/LogOut
        [Authorize, HttpPost("LogOut")]
		public async Task<IActionResult> LogOut()
		{
			await _signInManager.SignOutAsync();
			return Ok(new
			{
				Status = true,
				Message = "Logged out successfully"
			});
		}



		

		[Authorize]
		[HttpGet("GetUser")]
        public async Task<ActionResult<UserProfileDto>> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(User);
            //var email = User.FindFirstValue(ClaimTypes.Email);
            //var user = await _userManager.FindByEmailAsync(email);
            return Ok(new UserProfileDto()
            {
				UserName=user.UserName,
                Email = user.Email,
				PhoneNumber=user.PhoneNumber,
				Password=user.PasswordHash,
                Image=$"{_apiBaseUrl}{ user.FilePath }"
                
            });
        }

		[Authorize]
        [HttpPut("UpdateUser")]
        public async Task<ActionResult> UpdateUser(UpdateUserProfileDto updateUserProfile)
        {
            var user = await _userManager.GetUserAsync(User);
            //var email = User.FindFirstValue(ClaimTypes.Email);
            //var user = await _userManager.FindByEmailAsync(email);
		
            if (user == null)
            {
                return NotFound(new { message = "المستخدم غير موجود." });
            }
            user.UserName = updateUserProfile.UserName;
            user.Email = updateUserProfile.Email;
            user.PhoneNumber = updateUserProfile.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok(new { message = "تم تعديل البيانات بنجاح !" });
			}
			else
			{
                var passwordErrors = string.Join(" ",result.Errors.Select(e => e.Description));
                return BadRequest(new { errors = passwordErrors });
            }
           
        }


		[Authorize]
		[HttpPost("AddAddress")]
		public async Task<ActionResult> AddAddress(AddressDto address)
		{
			var user = await _userManager.GetUserAsync(User);
            //var email = User.FindFirstValue(ClaimTypes.Email);
            //var user2 = await _userManager.FindByEmailAsync(email);

			Address address1 = new Address()
			{
                FullName=address.FullName,
				PhoneNumber=address.PhoneNumber,
				City=address.City,
				Region=address.Region,
				Country=address.Country,
				AddressDetails=address.AddressDetails,
				AppUserId=user.Id
			};
			try
			{
                _unitOfWork.generic.Add(address1);
                _unitOfWork.Complet();
                return Ok(new
				{
					Message = "تم إضافه العنوان بنجاح!"
				});
			}
			catch (Exception)
			{

				return BadRequest(new
				{
                    Message = "لم تم إضافه العنوان !"
                });
			}
			




        }

        [Authorize]
        [HttpPut("EditAddress")]
        public async Task<ActionResult> EditAddress(AddressDto address)
        {
            var user = await _userManager.GetUserAsync(User);
            //var email = User.FindFirstValue(ClaimTypes.Email);
            //var user2 = await _userManager.FindByEmailAsync(email);

            
            Address address1 = new Address()
            {
                FullName = address.FullName,
                PhoneNumber = address.PhoneNumber,
                City = address.City,
                Region = address.Region,
                Country = address.Country,
                AddressDetails = address.AddressDetails,
                AppUserId = user.Id
            };
            try
            {
                _unitOfWork.generic.Update(address1);
                _unitOfWork.Complet();
                return Ok(new
                {
                    Message = "تم تعديل العنوان بنجاح!"
                });
            }
            catch (Exception)
            {

                return BadRequest(new
                {
                    Message = "لم يتم تعديل العنوان !"
                });
            }





        }

        [Authorize]
        [HttpGet("GetAddress")]
        public async Task<ActionResult<AddressDto>> GetAddress(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            //var email = User.FindFirstValue(ClaimTypes.Email);
            //var user2 = await _userManager.FindByEmailAsync(email);
            try
            {
            var address = await _unitOfWork.generic.GetByIdAsync(id);
                if (address==null)
                {
                    return NotFound(new ApiResponse(400,"هذا العنوان غير موجود"));
                }
                //AddressDto address1 = new AddressDto()
                //{

                //    FullName = address.FullName,
                //    PhoneNumber = address.PhoneNumber,
                //    City = address.City,
                //    Region = address.Region,
                //    Country = address.Country,
                //    AddressDetails = address.AddressDetails,

                //};

                var addressmaped = _mapper.Map<Address, AddressDto>(address);
                return Ok(addressmaped);
            }
            catch (Exception)
            {

               
            } return BadRequest(new
                {
                    Message = "لا يوجد عنوان لهذا المستخدم"
                });

        }

        [Authorize]
        [HttpGet("GetUserAddressess")]
        public async Task<ActionResult<GetAllAddressDto>> GetUserAddressess()
        {
            var user = await _userManager.GetUserAsync(User);
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user2 = await _userManager.FindByEmailAsync(email);
            try
            {

                var addresses = await _addressRepository.GetAllUserAddress(user.Id);
                var addressesMapped=  _mapper.Map<IEnumerable<Address>, IEnumerable<GetAllAddressDto>>(addresses);


                return Ok(addressesMapped);
            }
            catch (Exception)
            {

                return BadRequest(new
                {
                    Message = "لم يتم تعديل العنوان !"
                });
            }

        }


        [Authorize]
        [HttpPut("AddProfilePhoto")]
        public async Task<ActionResult> AddProfilePhoto(UserProfilePhotoDto userProfilePhotoDto)
        {
            var user=await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(new
                {
                    Message = "المستخدم غير موجود"
                });
            }
            string uploadDir = Path.Combine(_imagepath, "assets","images","Users");
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir); 
            }

            var photoName = $"{Guid.NewGuid()}{Path.GetExtension(userProfilePhotoDto.Image.FileName)}";
            var path=Path.Combine(uploadDir, photoName);

            // Attempt to delete the existing photo if it exists
                var existingPhotoPath = Path.Combine(_imagepath, user.FilePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (!string.IsNullOrEmpty(user.FilePath) && System.IO.File.Exists(existingPhotoPath))
            {
                try
                {
                    System.IO.File.Delete(existingPhotoPath);
                }
                catch (Exception ex)
                {
                    // Handle file deletion error
                    return StatusCode(500, new { message = "خطأ في حذف الصورة القديمة.", error = ex.Message });
                }
            }
                // Save the new photo
                try
                {
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await userProfilePhotoDto.Image.CopyToAsync(fileStream);
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { message = "خطأ في تحميل الصورة الجديدة.", error = ex.Message });
                }

                // Update the user's photo path
                user.FilePath = $"/assets/images/Users/{photoName}";
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok(new { message = "تم تعديل صوره الاكونت بنجاح !" });
                }
                else
                {
                    var errors = string.Join(" ", result.Errors.Select(e => e.Description));
                    return BadRequest(new { errors });
                }
            
          
         



        }
     

        //[Authorize]
        //[HttpGet("logout")]
        //public async Task<ActionResult> Logout()
        //{
        //	try
        //	{
        //		var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
        //		var result = await _userManager.RemoveAuthenticationTokenAsync(user, "JwtBearer", "JwtBearer");
        //		if (!result.Succeeded)
        //		{
        //			var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        //			return BadRequest($"خطأ في إزالة رمز المصادقة: {errors}");
        //		}
        //	}
        //	catch (Exception ex)
        //	{

        //		return BadRequest(ex.Message);
        //	}

        //	return Ok(new { Message = "تم تسجيل الخروج بنجاح" });
        //}

    }

}
