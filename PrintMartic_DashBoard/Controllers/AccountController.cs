using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrintMartic_DashBoard.ViewModels;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.DTOS;
using System.Security.Claims;



namespace PrintMartic_DashBoard.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Signin()
        {

            ClaimsPrincipal claimsPrincipal =HttpContext.User;
            if (claimsPrincipal.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signin(LoginViewModel login)
        {
            var user = await _userManager.FindByNameAsync(login.UserName);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Email Is Invalid");
                return RedirectToAction(nameof(Signin));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
           
                if (result.Succeeded)
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, login.UserName),
                     new Claim(ClaimTypes.NameIdentifier, login.UserName),
                     
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties authenticationProperties = new AuthenticationProperties()
                {
                    AllowRefresh =true,
                    IsPersistent = login.RememberMe
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authenticationProperties);

                return RedirectToAction("Index", "Home");
            }

            ViewData["ValidateMessage"] = "User Not Found";
            return View();
        }

        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Signin));
        }
        //public async Task<IActionResult> Logout()
        //{
        //    await _signInManager.SignOutAsync();
        //    return RedirectToAction(nameof(Login));
        //}




    }
}
