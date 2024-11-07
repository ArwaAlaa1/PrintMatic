using PrintMatic.Core;
using PrintMatic.Core.Repository.Contract;
using PrintMatic.Core.Services;
using PrintMatic.Helper;
using PrintMatic.Repository;
using PrintMatic.Repository.Repository;
using PrintMatic.Services;
using System.Runtime.CompilerServices;

namespace PrintMatic.Extentions
{
    public static class ApplicationServicesExtention
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
           
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped(typeof(IOrderService), typeof(OrderService));
            services.AddScoped(typeof(IOrderRepository), typeof(OrderRepository));
            services.AddAutoMapper(typeof(MappingProfiles));
			services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IAddressRepository), typeof(AddressRepository));
            services.AddScoped<IEmailService, EmailService>();
			services.AddMemoryCache();
            services.AddScoped<CodeVerificationService>();
            services.AddScoped<CustomeUpload>();
            return services;
            
        }
    }
}
