using Microsoft.AspNetCore.Identity;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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


			

			}
		}
	
		public static async Task SeedShippingCost(PrintMaticContext _Context)
		{
			
			if (_Context.ShippingCosts.Count() == 0)
			{
                var costs = File.ReadAllText(".././PrintMatic.Repository/Data/DataSeed/ShippingCost.json");
                var methods = JsonSerializer.Deserialize<List<ShippingCost>>(costs);
                if (methods.Count() > 0)
                {
                    foreach (var item in methods)
                    {
                        _Context.Set<ShippingCost>().Add(item);
                    }
                    await _Context.SaveChangesAsync();

                }
            }
		}
	
	}
}
