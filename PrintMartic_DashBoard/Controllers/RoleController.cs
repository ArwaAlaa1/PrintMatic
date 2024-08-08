using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrintMartic_DashBoard.ViewModels;

namespace PrintMartic_DashBoard.Controllers
{
	public class RoleController : Controller
	{
		private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;

        public RoleController(RoleManager<IdentityRole> roleManager,IMapper mapper)
		{
			this.roleManager = roleManager;
            this.mapper = mapper;
        }
		public async Task<IActionResult> Index()
		{
			var Roles = await roleManager.Roles.ToListAsync();
			return View(Roles);
		}


        // GET: RoleController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleViewModel.RoleName);
                try
                {
                    if (!roleExist)
                    {
                        var mappedrole = mapper.Map<RoleViewModel,IdentityRole>(roleViewModel);
                        await roleManager.CreateAsync(mappedrole);

                    }
                    else
                    {
                        ModelState.AddModelError("Name", "Role Name Is Exist");
                        return View(roleViewModel);
                    }

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            await roleManager.DeleteAsync(role);
            return RedirectToAction(nameof(Index));
        }

        //public async Task<IActionResult> Edit(string id)
        //{
        //    var role = await roleManager.FindByIdAsync(id);
        //    var mappedRole = new RoleViewModel()
        //    {
        //        Name = role.Name
        //    };
        //    return View(mappedRole);
        //}
    }

}
