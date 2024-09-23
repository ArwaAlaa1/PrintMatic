using AutoMapper;
using PrintMatic.Core.Entities;
using PrintMatic.DTOS;

namespace PrintMatic.Helper
{
<<<<<<< HEAD
    public class MappingProfiles:Profile
    {

        public MappingProfiles() 
        { 
        
           CreateMap<CustomerCart,CustomerCartDto>().ReverseMap();
            CreateMap<CartItems,CartItemsDto>().ReverseMap();

            CreateMap<CategoryDTO, Category>().ReverseMap();
            CreateMap<Category , CategoryWithProDetails>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ProductSale , ProductSaleDto>().ForMember(x => x.ProductId, o => o.MapFrom(x => x.Product.Id)).ForMember(x => x.ProductAfterSale, o => o.MapFrom(x => x.PriceAfterSale) ).ReverseMap();
            CreateMap<ProductPhotos, ProductPhotoDto>().ForMember(x => x.ProductId, o => o.MapFrom(x => x.Product.Id)).ForMember(x =>x.FilePath , o => o.MapFrom(x => x.FilePath)).ReverseMap();
>>>>>>> ce0301606de742a5cf94105f56ef58c8b53397f8

        }
    }
}
