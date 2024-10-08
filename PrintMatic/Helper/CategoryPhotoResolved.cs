using AutoMapper;
using PrintMatic.Core.Entities;
using PrintMatic.DTOS;

namespace PrintMatic.Helper
{
    public class CategoryPhotoResolved : IValueResolver<Category, CategoryDTO, string>
    {
        private readonly IConfiguration configuration;

        public CategoryPhotoResolved(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string Resolve(Category source, CategoryDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PhotoURL))
                return $"{configuration["DashboardUrl"]}//Uploads//category//{source.PhotoURL}";

            return string.Empty;
        }
    }
    
}
