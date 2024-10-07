using Microsoft.AspNetCore.Identity;
using PrintMatic.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Repository.Data
{
	public static class AppSeeding
	{
		public static async Task SeedUsersAsync(UserManager<AppUser> _usermanager,RoleManager<IdentityRole> _roleManager)
		{
			if (_usermanager.Users.Count() == 0)
			{
				var arwa = new AppUser()
				{
					Email="arwaalaa99@hotmail.com",
					UserName="Arwa10",
					PhoneNumber="01011037481"
				};
<<<<<<< HEAD
				var nagham = new AppUser()
				{
					Email = "nagham15@gmail.com",
					UserName = "nagham",
					PhoneNumber = "01011037481"
				};

				var role = new IdentityRole()
				{
					
					Name="Admin"
				};

				var result=await _usermanager.CreateAsync(arwa,"P@ssWord1");
				var result2 = await _usermanager.CreateAsync(nagham, "P@ssWord1");
				var Roleresult = await _roleManager.CreateAsync(role);
				var resultAdd =await _usermanager.AddToRoleAsync(arwa, "Admin");
				var resultAdd2 = await _usermanager.AddToRoleAsync(nagham, "Admin");

=======
				var result=await _usermanager.CreateAsync(user,"P@ssWord1");
>>>>>>> e7a1ce2fcefa8289c004219ba33f26f0fd9a5f0d
			}
		}
	}
}
