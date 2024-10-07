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
		public static async Task SeedUsersAsync(UserManager<AppUser> _usermanager)
		{
			if (_usermanager.Users.Count()==0)
			{
				var user = new AppUser()
				{
					Email="arwaalaa24682468@gmail.com",
					UserName="ArwaAlaa",
					PhoneNumber="01011037481"
				};
				var result=await _usermanager.CreateAsync(user,"P@ssWord1");
			}
		}
	}
}
