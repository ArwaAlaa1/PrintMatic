using AutoMapper;
using PrintMatic.Core.Entities.Order;
using PrintMatic.DTOS.OrderDTOS;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace PrintMatic.Helper
{
    public class MappingOrder : Profile
    {
        public MappingOrder()
        {
            // OrderItem -> OrderItemInDelivery Mapping
            CreateMap<OrderItem, OrderItemInDelivery>()
           .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.OrderItemStatus, opt => opt.MapFrom(src =>
               src.OrderItemStatus.GetType().GetMember(src.OrderItemStatus.ToString())
                   .FirstOrDefault()
                   .GetCustomAttribute<EnumMemberAttribute>().Value))
           .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ProductItem.Name))
           //.ForMember(dest => dest.CompanyName, opt => opt.MapFrom<CompanyNameResolver>())
           //.ForMember(dest => dest.CompanyLocation, opt => opt.MapFrom<CompanyLocationResolver>())
           .ForPath(dest => dest.Photo, opt => opt.MapFrom(src => src.ProductItem.Photo))
           .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
           .ReverseMap();



            CreateMap<Order, DeliveryOrderDto>()
     .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src =>
         src.OrderDate.ToString("dddd, dd MMMM yyyy", new CultureInfo("ar"))))
     .ForMember(dest => dest.OrderNum, opt => opt.MapFrom(src => src.Id + 1000))
     .ForMember(dest => dest.TraderStatus, opt => opt.MapFrom(src =>
         src.StatusReady.GetType().GetMember(src.StatusReady.ToString())
             .FirstOrDefault()
             .GetCustomAttribute<EnumMemberAttribute>().Value))
     .ForMember(dest => dest.OrderStatus, opt => opt.MapFrom(src =>
         src.Status.GetType().GetMember(src.Status.ToString())
             .FirstOrDefault()
             .GetCustomAttribute<EnumMemberAttribute>().Value))
     .ForPath(dest => dest.ShippingAddress.FullName, opt => opt.MapFrom(src => src.ShippingAddress.FullName))
     .ForPath(dest => dest.ShippingAddress.PhoneNumber, opt => opt.MapFrom(src => src.ShippingAddress.PhoneNumber))
     .ForPath(dest => dest.ShippingAddress.City, opt => opt.MapFrom(src => src.ShippingAddress.City))
     .ForPath(dest => dest.ShippingAddress.Region, opt => opt.MapFrom(src => src.ShippingAddress.Region))
     .ForPath(dest => dest.ShippingAddress.Country, opt => opt.MapFrom(src => src.ShippingAddress.Country))
     .ForPath(dest => dest.ShippingAddress.AddressDetails, opt => opt.MapFrom(src => src.ShippingAddress.AddressDetails))
     .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.OrderItems.Count))
     .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.SubTotal + src.ShippingCost.Cost))
     .ForMember(dest => dest.ShippingCost, opt => opt.MapFrom(src => src.ShippingCost.Cost))
     .ReverseMap();

        }
    }

}
