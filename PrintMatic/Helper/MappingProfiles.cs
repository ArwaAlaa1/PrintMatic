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
            CreateMap<CartItemsDto, CartItems>().ReverseMap();

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
             .ForMember(dest => dest.OrderItemPhoto, opt => opt.MapFrom(src => src.OrderItems.Select(p => p.ProductItem.Photo)));

            CreateMap<Order, OneOrderReturnDto>()
                  //.ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src =>
                  //    src.OrderDate.ToString("dddd, dd MMMM yyyy", new CultureInfo("ar"))))
                  .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
                  .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
                      src.Status.GetType().GetMember(src.Status.ToString())
                          .FirstOrDefault()
                          .GetCustomAttribute<EnumMemberAttribute>().Value ))
                  .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.OrderItems.Count()))
                  .ForPath(dest => dest.orderSummary.TotalItems, opt => opt.MapFrom(src => src.OrderItems.Sum(item => item.Quantity)))
                  .ForPath(dest => dest.orderSummary.TotalPriceBeforeDiscount, opt => opt.MapFrom(src =>
                      src.OrderItems.Sum(oi => oi.ProductItem.Price)))
                  .ForPath(dest => dest.orderSummary.TotalPriceAfterDiscount, opt => opt.MapFrom(src => src.SubTotal))
                  .ForPath(dest => dest.orderSummary.ShippingPrice, opt => opt.MapFrom(src => src.ShippingCost.Cost))
                  .ForPath(dest => dest.orderSummary.FinalTotal, opt => opt.MapFrom(src => src.GetTotal()))
                  .ForPath(dest => dest.ShippingAddress.City, opt => opt.MapFrom(src => src.ShippingAddress.City))
                  .ForPath(dest => dest.ShippingAddress.Country, opt => opt.MapFrom(src => src.ShippingAddress.Country))
                  .ForPath(dest => dest.ShippingAddress.Region, opt => opt.MapFrom(src => src.ShippingAddress.Region))
                  .ForPath(dest => dest.ShippingAddress.AddressDetails, opt => opt.MapFrom(src => src.ShippingAddress.AddressDetails))
                  .ReverseMap();


            CreateMap<OrderItem, OrderItemInOrderReturnDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductItem.ProductId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductItem.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.ProductItem.Price))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.PriceAfterSale, opt => opt.MapFrom(src => src.ProductItem.PriceAfterSale))
                .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.ProductItem.Photo)).ReverseMap();

            CreateMap<OrderItem, OrderItemReturnDto>()
               .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ItemType, opt => opt.MapFrom(src =>
                      src.ProductItem.ItemType.GetType().GetMember(src.ProductItem.ItemType.ToString())
                          .FirstOrDefault()
                          .GetCustomAttribute<EnumMemberAttribute>().Value))
                 .ForMember(dest => dest.OrderItemStatus, opt => opt.MapFrom(src =>
                      src.OrderItemStatus.GetType().GetMember(src.OrderItemStatus.ToString())
                          .FirstOrDefault()
                          .GetCustomAttribute<EnumMemberAttribute>().Value))
                 .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductItem.ProductId))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductItem.Name))
               .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.ProductItem.Price))
               .ForMember(dest => dest.PriceAfterSale, opt => opt.MapFrom(src => src.ProductItem.PriceAfterSale))
               .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => src.ProductItem.Photo))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.ProductItem.Color))
          .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.ProductItem.Size))
          .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.ProductItem.Text))
          .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.ProductItem.Date))
          .ForMember(dest => dest.FilePdf, opt => opt.MapFrom(src => src.ProductItem.FilePdf)).ReverseMap();
           

            CreateMap<Favorite, FavouriteDto>().ReverseMap();
            CreateMap<ProductColor, ColorDto>().ReverseMap();
            CreateMap<ProductSize, SizeDto>().ReverseMap();

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
