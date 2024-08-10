using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrintMartic_DashBoard.ViewModels;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.DTOS;
using System.Security.Claims;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace PrintMartic_DashBoard.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AdminController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
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


    

      
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            // Step 1: Find the user by username
            var user = await _userManager.FindByNameAsync(login.UserName);
            if (user == null)
            {
                // Step 2: Handle case where the user is not found
                ModelState.AddModelError("Email", "Email is invalid");
                return View(login); // Return the view with the model to display validation errors
            }

            // Step 3: Check if the provided password is correct
            var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
            if (!result.Succeeded)
            {
                // Step 4: Handle case where password is incorrect
                ModelState.AddModelError(string.Empty, "You are not authorized");
                return View(login); // Return the view with the model to display validation errors
            }

            // Step 5: If credentials are correct, create claims and sign in the user
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, login.UserName),
        // You can add additional claims here, such as roles
        // new Claim(ClaimTypes.Role, "Admin")
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Remember me
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60) // Set the expiration time for the cookie
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Step 6: Redirect to the home page after successful login
            return RedirectToAction("Index", "Home");
        }

        //[HttpPost]
        //public async Task<IActionResult> Login(LoginViewModel login)
        //{
        //    var user = await _userManager.FindByNameAsync(login.UserName);
        //    if (user == null)
        //    {
        //        ModelState.AddModelError("Email", "Email Is Invalid");
        //        return RedirectToAction(nameof(Login));
        //    }
        //    var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
        //    if (!result.Succeeded)
        //    {
        //        ModelState.AddModelError(string.Empty, "you Are Not Authorized");
        //        return RedirectToAction(nameof(Login));
        //    }
        //    else
        //    {
        //        var claims = new List<Claim>
        //        {
        //            new Claim(ClaimTypes.Name, login.UserName),
        //           // new Claim(ClaimTypes.Role, "Admin") // Example role
        //        };
        //        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //        var authProperties = new AuthenticationProperties
        //        {
        //            IsPersistent = true, // Remember me
        //            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
        //        };

        //        await HttpContext.SignInAsync(
        //            CookieAuthenticationDefaults.AuthenticationScheme,
        //            new ClaimsPrincipal(claimsIdentity),
        //            authProperties);
        //        return RedirectToAction("Index", "Home");
        //    }

        //}

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
