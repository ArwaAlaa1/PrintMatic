using AutoMapper;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.DTOS;

namespace PrintMatic.Helper
{
    public class MappingProfiles : Profile
    {

        public MappingProfiles()
        {
            CreateMap<CategoryDTO, Category>().ReverseMap();
            CreateMap<Category , CategoryWithProDetails>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, ProductDetailsDTO>().ForMember(x => x.CategoryName , o => o.MapFrom(x => x.Category.Name));
            CreateMap<AppUser , UserSimpleDetails>().ReverseMap();
        }
    }
}
