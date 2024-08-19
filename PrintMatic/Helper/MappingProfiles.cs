using AutoMapper;
using PrintMatic.Core.Entities;
using PrintMatic.DTOS;

namespace PrintMatic.Helper
{
    public class MappingProfiles:Profile
    {

        public MappingProfiles() 
        { 
        
           CreateMap<CustomerCart,CustomerCartDto>().ReverseMap();
            CreateMap<CartItems,CartItemsDto>().ReverseMap();

        }
    }
}
