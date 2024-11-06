using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Org.BouncyCastle.Asn1.Cms;
using PrintMatic.Core;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.Core.Repository.Contract;
using PrintMatic.Core.Services;
using PrintMatic.DTOS.IdentityDTOS;
using PrintMatic.Errors;
using PrintMatic.Extensions;
using PrintMatic.Extentions;
using PrintMatic.Helper;
using PrintMatic.Repository;
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
        private readonly IUnitOfWork _unitOfWork;
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
            IUnitOfWork unitOfWork,
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
				await _userManager.FindByNameAsync(loginDto.EmailOrUserName) ;
                               
            if (user == null) return Unauthorized(new {
				Message = "اسم المستخدم أو البريد الالكترونى غير صحيح"
			});

			var result = await _signInManger.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
				return Unauthorized(new
				{
				
					Message = " كلمة المرور غير صحيحة"
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
            if (emailexsist != null)
            {
                return new BadRequestObjectResult(new  { Message= "البريد الإلكتروني مستخدم بالفعل" } );
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
				string errors = string.Join(", ", result.Errors.Select(e => _userService.GetCustomErrorsMessage(e)));

				
				return BadRequest(new { message = errors });

				//if (result.Errors.Any(e => e.Code == "DuplicateUserName"))
				//{
				//	return new BadRequestObjectResult(new { Message = "اسم المستخدم موجود بالفعل. الرجاء اختيار اسم مستخدم آخر." });
				//}
				//if (result.Errors.Any(e => e.Code == "DuplicateUserName"))
				//{
				//	return new BadRequestObjectResult(new { Message = "اسم المستخدم موجود بالفعل. الرجاء اختيار اسم مستخدم آخر." });
				//}
				//var errors = result.Errors.Select(e => _userService.GetCustomErrorsMessage(e)).ToArray();

				//return new BadRequestObjectResult(new { Message = "فشل إنشاء المستخدم. الرجاء التحقق من البيانات والمحاولة مرة أخرى." });
			}

		
            return Ok(new UserDto()
            {
                UserName=user.UserName,
                Email = user.Email,
                Token = await _tokenService.CreateToken(user, _userManager)
            });
        }

      

		//ForgetPassword EndPoint Domain/Api/Account/forgetpassword
		
        [HttpPost("forgetpassword")]
        public async Task<ActionResult> ForgetPassword(ForgetPasswordDTO forgetPasswordDto)
        {
			
			var user = await _userManager.FindByEmailAsync(forgetPasswordDto.Email);
			if (user == null)
			{
				
				return BadRequest(new { Message = "هذا البريد الالكترونى غير موجود" });
			}

			var OTP = new Random().Next(1000, 9999).ToString();
           
            await _verificationService.SaveVerificationCodeAsync(user.Id, OTP);

			await _emailService.SendEmailAsync(user.Email, " رمز التحقق من giftly", OTP);

			return Ok(new
            {
                Message = "!تم إرسال الكود بنجاح "
            });
		}

		//VerifyCode EndPoint Domain/Api/Account/VerifyCode
		
        [HttpPost("VerifyCode")]
        public async Task<ActionResult> VerifyCode(VerifyCodeDto verifyCode)
        {
			var user = await _userManager.FindByEmailAsync(verifyCode.Email);

			if (user is null)
				return Unauthorized(new { Message = "هذا البريد الالكترونى غير موجود" });


			var otp = await _verificationService.GetVerificationCode(user.Id);
			
			if (otp == null)
                return BadRequest(new { Message = "تم انتهاء صلاحيه استخدام الكود" });
           
            else if(otp != verifyCode.Code)
                return BadRequest(new { Message = "هذا الكود غير صحيح" });
			else
				return Ok(new { Message = "تم تأكيد الكود بنجاح" });




		}

		//ResendCode EndPoint Domain/Api/Account/ResendCode
		[HttpGet("ResendCode")]
        public async Task<ActionResult> ResendCode(EmailDto emailDto)
        {
			var user = await _userManager.FindByEmailAsync(emailDto.Email);
			
			if (user is null)
				return Unauthorized(new { Message = "هذا البريد الالكترونى غير موجود" });


			var OTP = new Random().Next(1000, 9999).ToString();

			await _verificationService.SaveVerificationCodeAsync(user.Id, OTP);

			await _emailService.SendEmailAsync(user.Email, " رمز التحقق من giftly", OTP);

			return Ok(new { Message = "!تم إرسال الكود بنجاح " });
		}

        //ResetPassword EndPoint Domain/Api/Account/ResetPassword
       
        [HttpPost("ResetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {

			var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);

			if (user is null)
				return Unauthorized(new { Message = "هذا البريد الالكترونى غير موجود" });

			var token = await _userManager.GeneratePasswordResetTokenAsync(user);
			if (string.IsNullOrEmpty(token))
			{
				return BadRequest(new { Message = "فشل في إنشاء رمز إعادة تعيين كلمة المرور" });
			}
			var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordDto.NewPassword);
			if (!result.Succeeded)
			{
			
				//var errors = result.Errors.Select(e => _userService.GetCustomErrorsMessage(e)).ToArray();
				
				return new BadRequestObjectResult(new {Message= "فشل في إعادة تعيين كلمة المرور" });
			}
			

			return Ok(new
			{
				
				Message="!تم تعديل كلمه السر بنجاح "
			});
		}

        //ChangePassword EndPoint Domain/Api/Account/ChangePassword
        [Authorize]
		[HttpPost("ChangePassword")]
		public async Task<ActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
		{

			var user = await _userManager.GetUserAsync(User);
			if (user is null)
				return Unauthorized(new { Message = "المستخدم غير مسجل الدخول" });

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
				//var errors = result.Errors.Select(e => _userService.GetCustomErrorsMessage(e)).ToArray();

				return new BadRequestObjectResult( new {Message= "فشل في إعادة تعيين كلمة المرور جديده" });
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
				
				Message = "تم تسجيل الخروج بنجاح"
			});
		}



		

		[Authorize]
		[HttpGet("GetUser")]
        public async Task<ActionResult<UserProfileDto>> GetCurrentUser()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return  NotFound(new
                {
                    Message = "هذا المستخدم غير موجود"
                });
            }
            //var email = User.FindFirstValue(ClaimTypes.Email);
            //var user = await _userManager.FindByEmailAsync(email);
            if (user.FilePath == null)
            {
                return Ok(new UserProfileDto()

                {
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
					Token=await _tokenService.CreateToken(user,_userManager),

                    Image = $"{_apiBaseUrl}/images/Users/{user.FilePath ?? "UserDefault.png"}"

                });
            }
			return Ok(new UserProfileDto()

			{
				UserName = user.UserName,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
                Token = await _tokenService.CreateToken(user, _userManager),


                Image = $"{_apiBaseUrl}{user.FilePath}"

			});
		}

		[Authorize]
        [HttpPut("UpdateUser")]
        public async Task<ActionResult> UpdateUser(UpdateUserProfileDto updateUserProfile)
        {

            try
            {
				var user = await _userManager.GetUserAsync(User);

				var emailexsist = await _userManager.FindByEmailAsync(updateUserProfile.Email);
                if (updateUserProfile.UserName==user.UserName&& updateUserProfile.Email==user.Email && updateUserProfile.PhoneNumber==user.PhoneNumber)
                {
					return new BadRequestObjectResult(new { Message = "هذه نفس بيانات المستخدم القديمه!" });
				}
				if (updateUserProfile.Email != user.Email)
				{
					if (emailexsist != null)
					{
						return new BadRequestObjectResult(new { Message = "البريد الإلكتروني مستخدم بالفعل" });
					}
				}
				var usernameExsist = await _userManager.FindByNameAsync(updateUserProfile.UserName);
				if (updateUserProfile.UserName != user.UserName)
				{
					if (usernameExsist != null)
					{

						return new BadRequestObjectResult(new { Message = "اسم المستخدم موجود بالفعل. الرجاء اختيار اسم مستخدم آخر." });
					}


				}

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
					return Ok(new { message = "تم تعديل البيانات بنجاح!" });
				}
				else
				{

					return BadRequest(new { message = "فشل في تعديل البيانات. الرجاء المحاولة مرة أخرى." });
				}
			}
            catch (Exception)
            {

				return BadRequest(new { message = "فشل في تعديل البيانات. الرجاء المحاولة مرة أخرى." });
			}


		}


		[Authorize]
		[HttpPost("AddAddress")]
		public async Task<ActionResult> AddAddress(AddressDto address)
		{
            
            var addressResult = await _userManager.AddAddressUser(_unitOfWork, address, User);
            if (addressResult >0)
            { return Ok(new
				{
					Message = "تم إضافه العنوان بنجاح!"
				});

            }else
                return BadRequest(new
				{
                    Message = "فشل إضافه العنوان !"
                });
	

        }

        [Authorize]
        [HttpPut("EditAddress")]
        public async Task<ActionResult> EditAddress(AddressUseIdDto address)

        {
          
            try
            {
                var user =await _userManager.GetUserAsync(User);
                var useraddress = await _addressRepository.GetUserAddress(address.Id);
				//var addressmaped = _mapper.Map<AddressDto, Address>(address);
                //useraddress = addressmaped;
				_mapper.Map(address, useraddress);
				_unitOfWork.Repository<Address>().Update(useraddress);
                var count=await _unitOfWork.Complet();
				if (count > 0)
				{
					return Ok(new
					{
						Message = "تم تعديل العنوان بنجاح!"
					});

				}
				else
					return BadRequest(new
					{
						Message = "فشل تعديل العنوان !"
					});

			}
			catch (Exception ex)
			{

				return BadRequest(new
				{
					Message = "لم يتم تعديل العنوان !"
				});

			}


        }

        [Authorize]
        [HttpGet("GetAddress")]
        public async Task<ActionResult<AddressUseIdDto>> GetAddress(int id)
        {
            //var user = await _userManager.GetUserAsync(User);

			try
			{
				var address = await _unitOfWork.Repository<Address>().GetByIdAsync(id);
				if (address == null)
				{
					return NotFound(new { Message = "هذا العنوان غير موجود" });
				}

				var addressmaped = _mapper.Map<Address, AddressUseIdDto>(address);
				return Ok(addressmaped);
			}
			catch (Exception)
			{
				return BadRequest(new
				{
					Message = "لا يوجد عنوان لهذا المستخدم"
				});


			}

		}

        [Authorize]
        [HttpGet("GetUserAddressess")]
        public async Task<ActionResult<AddressUseIdDto>> GetUserAddressess()
        {
            var user = await _userManager.GetUserAsync(User);
        
            try
            {

                var addresses = await _addressRepository.GetAllUserAddress(user.Id);
                var addressesMapped=  _mapper.Map<IEnumerable<Address>, IEnumerable<AddressUseIdDto>>(addresses);

                if (addressesMapped.Count()!=0)
                {
                    return Ok(addressesMapped);

                }
                else
                    return NotFound(new { Message = " لا يوجد عنوان " });
                
            }
            catch (Exception)
            {

                return BadRequest(new
                {
                    Message = "حدث خطأ أثناء استرجاع العناوين. الرجاء المحاولة لاحقاً."
				});
            }

        }


		[Authorize]
		[HttpPut("AddProfilePhoto")]
		public async Task<ActionResult> AddProfilePhoto(UserProfilePhotoDto userProfilePhotoDto)
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound(new { Message = "المستخدم غير موجود" });
			}

			// Ensure the images are saved in the correct wwwroot path
			string uploadDir = Path.Combine(_imagepath, "images", "Users"); // Adjusted path
			if (!Directory.Exists(uploadDir))
			{
				Directory.CreateDirectory(uploadDir);
			}

			var photoName = $"{Guid.NewGuid()}{Path.GetExtension(userProfilePhotoDto.Image.FileName)}";
			var path = Path.Combine(uploadDir, photoName);

			// Check if the user already has an existing photo and delete it
			if (!string.IsNullOrEmpty(user.FilePath))
			{
				var existingPhotoPath = Path.Combine(_imagepath, user.FilePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
				if (System.IO.File.Exists(existingPhotoPath))
				{
					try
					{
						System.IO.File.Delete(existingPhotoPath);
					}
					catch (Exception ex)
					{
						return BadRequest(new { Message = "خطأ في حذف الصورة القديمة." });
					}
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
				return BadRequest(new { message = "خطأ في تحميل الصورة الجديدة." });
			}

			// Update the user's photo path
			user.FilePath = $"/images/Users/{photoName}";
			var result = await _userManager.UpdateAsync(user);
			if (result.Succeeded)
			{
				return Ok(new { message = "تم رفع صورة الحساب بنجاح!" });
			}
			else
			{
				return BadRequest(new { Message = "حدث خطأ في إضافة صورة الحساب. أعد المحاولة مرة أخرى." });
			}
		}

		

	}

}
