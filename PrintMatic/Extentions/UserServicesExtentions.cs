using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.DTOS.IdentityDTOS;
using PrintMatic.Repository;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace PrintMatic.Extentions
{
	public static class UserServicesExtentions
	{

		public async static Task<int> AddAddressUser(this UserManager<AppUser> userManager, IUnitOfWork unitOfWork, AddressDto address, ClaimsPrincipal User)
		{
			var user = await userManager.GetUserAsync(User);
			//var email = User.FindFirstValue(ClaimTypes.Email);
			//var user2 = await _userManager.FindByEmailAsync(email);

			Address address1 = new Address()
			{
				
				FullName = address.FullName,
				PhoneNumber = address.PhoneNumber,
				City = address.City,
				Region = address.Region,
				Country = address.Country,
				AddressDetails = address.AddressDetails,
				AppUserId = user.Id
			};
			try
			{
				unitOfWork.Repository<Address>().Add(address1);
				return await unitOfWork.Complet();
			}
			catch (Exception ex)
			{
				return 0;

			}

		}


		public async static Task<int> EditAddressUser(this UserManager<AppUser> userManager, IUnitOfWork unitOfWork, AddressDto addressDto, ClaimsPrincipal User,int id)
		{
			var user = await userManager.GetUserAsync(User);
			//var email = User.FindFirstValue(ClaimTypes.Email);
			//var user2 = await _userManager.FindByEmailAsync(email);
			var address = await unitOfWork.Repository<Address>().GetByIdAsync(id);

			 address = new Address()
			{
				
				FullName = addressDto.FullName,
				PhoneNumber = addressDto.PhoneNumber,
				City = addressDto.City,
				Region = addressDto.Region,
				Country = addressDto.Country,
				AddressDetails = addressDto.AddressDetails,
				AppUserId = user.Id
			};
			try
			{
				unitOfWork.Repository<Address>().Update(address);

				return await unitOfWork.Complet();
			}
			catch (Exception ex)
			{
				return 0;

			}

		}

		public async static Task<int> FindUserWithAddress(this UserManager<AppUser> userManager,AddressDto address,int id, ClaimsPrincipal User,IUnitOfWork unitOfWork)
		{
			//var user = await userManager.GetUserAsync(User);
		var user = userManager.Users
		.Include(u => u.Addresses)
		.SingleOrDefault(u => u.Addresses!.Any(a => a.Id ==id));
			
			var address01 = new Address()
			{
				
				FullName = address.FullName,
				PhoneNumber = address.PhoneNumber,
				City = address.City,
				Region = address.Region,
				Country = address.Country,
				AddressDetails = address.AddressDetails,
				AppUserId = user.Id
			};
			var addressuser = user.Addresses.Where(a => a.Id == id).FirstOrDefault();
			addressuser= address01;
			 unitOfWork.Repository<Address>().Update(addressuser);

			return await unitOfWork.Complet();
		
		}
	
	}
}
