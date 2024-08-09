using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PrintMartic_DashBoard.Helper.ViewModels;
using PrintMartic_DashBoard.ViewModels;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Identity;

namespace PrintMartic_DashBoard.Helper
{
	public class MappingProfiles: Profile
	{

		public MappingProfiles()
		{
			CreateMap<CategoryVM, Category>().ReverseMap();
            CreateMap<SaleVM, Sale>().ReverseMap();
			CreateMap<ProductPhotos, ProductPhotosVM>().ReverseMap();
            CreateMap<Review, ReviewVM>().ReverseMap();
            CreateMap<ProductSale, ProductSaleVM>().ReverseMap();
            CreateMap<Product, ProductVM>().ReverseMap();

            CreateMap<IdentityRole, RoleViewModel>().ReverseMap();

            CreateMap<AddUserFormViewModel, AppUser>().ReverseMap();

        }
    }
}
