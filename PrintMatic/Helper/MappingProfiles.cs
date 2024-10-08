using AutoMapper;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.DTOS;
using PrintMatic.DTOS.IdentityDTOS;

namespace PrintMatic.Helper
{

    public class MappingProfiles:Profile
    {

        public MappingProfiles() 
        { 
        
           CreateMap<CustomerCart,CustomerCartDto>().ReverseMap();
            CreateMap<CartItems,CartItemsDto>().ReverseMap();
            CreateMap<Address, AddressUseIdDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
            CreateMap<Favorite, FavouriteDto>().ReverseMap();
            CreateMap<ProductColor, ColorDto>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ForMember(i => i.PhotoURL, i => i.MapFrom<CategoryPhotoResolved>()).ReverseMap();
            CreateMap<Category , CategoryWithProDetails>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, ProductDetailsDTO>().ReverseMap();

            // CreateMap<ProductSale , ProductSaleDto>().ForMember(x => x., o => o.MapFrom(x => x.Product.Id)).ForMember(x => x.ProductAfterSale, o => o.MapFrom(x => x.PriceAfterSale) ).ReverseMap();
            CreateMap<ProductPhotos, ProductPhotoDto>().ForMember(x => x.ProductId, o => o.MapFrom(x => x.Product.Id)).ForMember(x =>x.FilePath , o => o.MapFrom(x => x.FilePath)).ReverseMap();
            

        }
        private void CreateMap<T1, T2>(T2 addressDto, string v, object id)
        {
            throw new NotImplementedException();
        }
    }
}
