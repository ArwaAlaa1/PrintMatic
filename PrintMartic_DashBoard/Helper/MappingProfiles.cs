﻿using AutoMapper;
using PrintMartic_DashBoard.Helper.ViewModels;
using PrintMatic.Core.Entities;

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




        }
    }
}
