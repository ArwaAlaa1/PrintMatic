using AutoMapper;
using Microsoft.OpenApi.Extensions;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.Core.Entities.Order;
using PrintMatic.DTOS;
using PrintMatic.DTOS.IdentityDTOS;
using PrintMatic.DTOS.OrderDTOS;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using Address = PrintMatic.Core.Entities.Identity.Address;

namespace PrintMatic.Helper
{

    public class MappingProfiles:Profile
    {

        public MappingProfiles() 
        { 
        
           CreateMap<CustomerCart,CustomerCartDto>().ReverseMap();
            CreateMap<CartItems,CartItemsDto>().ReverseMap();
            CreateMap<Core.Entities.Identity.Address, AddressUseIdDto>().ReverseMap();
            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<Address, Core.Entities.Order.Address>().ReverseMap();
            CreateMap<ShippingCostDto, ShippingCost>().ReverseMap();
            CreateMap<Order, OrderReturnDto>()
             .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src =>
                 src.OrderDate.ToString("dddd, dd MMMM yyyy ", new CultureInfo("ar"))))
             .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.GetType().GetMember(src.Status.ToString()).FirstOrDefault().GetCustomAttribute<EnumMemberAttribute>().Value))
             .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.GetTotal()))
              .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.OrderItems.Count))
             .ForMember(dest => dest.OrderItemPhoto, opt => opt.MapFrom(src => src.OrderItems.FirstOrDefault().ProductItem.Photo));
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
