using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrintMartic_DashBoard.ViewModels;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.DTOS;
using System.Security.Claims;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.AspNetCore.Authorization;


namespace PrintMartic_DashBoard.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
           _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }





        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            var user = await _userManager.FindByNameAsync(login.UserName);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Email Is Invalid");
                return RedirectToAction(nameof(Login));
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "you Are Not Authorized");
                return RedirectToAction(nameof(Login));
            }
            else
            {
                
                

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, login.UserName),
                   // new Claim(ClaimTypes.Role, "Admin") // Example role
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, // Remember me
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                return RedirectToAction("Index", "Home");


            }

        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }




    }
}
