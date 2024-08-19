using PrintMatic.Core.Repository.Contract;
using PrintMatic.Helper;
using PrintMatic.Repository.Repository;
using PrintMatic.Services;
using System.Runtime.CompilerServices;

namespace PrintMatic.Extentions
{
    public static class ApplicationServicesExtention
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddAutoMapper(typeof(MappingProfiles));
            return services;
            
        }
    }
}
