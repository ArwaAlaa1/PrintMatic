using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrintMartic_DashBoard.ViewModels;

namespace PrintMartic_DashBoard.Controllers
{
	public class RoleController : Controller
	{
		private readonly RoleManager<IdentityRole> roleManager;

		public RoleController(RoleManager<IdentityRole> roleManager)
		{
			this.roleManager = roleManager;
		}
		public async Task<IActionResult> Index()
		{
			var Roles = await roleManager.Roles.ToListAsync();
			return View(Roles);
		}

        [HttpPost]
        public async Task<IActionResult> Create(RoleFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var RoleExsits = await roleManager.RoleExistsAsync(model.Name);
                if (!RoleExsits)
                {
                    await roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));
                    return RedirectToAction(nameof(Index), await roleManager.Roles.ToListAsync());
                }
                else
                {
                    ModelState.AddModelError("Name", "Role Is Exists");
                    return View(nameof(Index), await roleManager.Roles.ToListAsync());


                }
            }
            return RedirectToAction(nameof(Index));
        }
    }

}
