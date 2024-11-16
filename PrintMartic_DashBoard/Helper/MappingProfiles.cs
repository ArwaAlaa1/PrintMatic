using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PrintMartic_DashBoard.Helper.ViewModels;
using PrintMartic_DashBoard.ViewModels;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.Core.Entities.Order;

namespace PrintMartic_DashBoard.Helper
{
	public class MappingProfiles: Profile
	{

		public MappingProfiles()
		{
			CreateMap<CategoryVM, Category>().ReverseMap();
            CreateMap<CategoryCrVM, Category>().ReverseMap();
            
            CreateMap<SaleVM, Sale>().ReverseMap();
			CreateMap<ProductPhotos, ProductPhotosVM>().ReverseMap();
            CreateMap<Review, ReviewVM>().ReverseMap();
            CreateMap<ProductSale, ProductSaleVM>().ReverseMap();
            CreateMap<Product, ProductVM>().ReverseMap();

            CreateMap<IdentityRole, RoleViewModel>().ReverseMap();

            CreateMap<UserFormViewModel, AppUser>().ReverseMap();
            CreateMap<RoleUserViewModel, AppUser>().ReverseMap();
            CreateMap<AppUser,UserViewModel>().ReverseMap();


            CreateMap<Order, OrderViewModelForCompany>().ForMember(dest => dest.NumberItems, src => src.MapFrom(o => o.OrderItems.Count));

            CreateMap<OrderItem, OrderItemVM>().ReverseMap();
                //.ForMember(dest => dest., src => src.MapFrom(o => o.OrderItems.Count));



        }
    }
}
