using Microsoft.AspNetCore.Identity;
using PrintMatic.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Repository.Identity
{
	public static class AppSeed
	{

		public static async Task UserSeedAsync(UserManager<AppUser> _usermanager)
		{
			if (_usermanager.Users.Count() == 0 )
			{
				var user = new AppUser()
				{
					DisplayName="Arwa Alaa",
					Email="arwaalaa99@hotmail.com",
					UserName="Arwa1",
					PhoneNumber="01011037481",
				
				};
			
				await _usermanager.CreateAsync(user, "P@ssWord");

			
				

			}
		}
	}
}
