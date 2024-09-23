using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.Core.Services;
using PrintMatic.Repository.Data;
using PrintMatic.Services;
using System.Text;


namespace PrintMatic.Extensions
{
    public static class IdentityServicesExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
			services.AddScoped<ITokenService, TokenService>();

			services.AddScoped<IUserService, UserService>();

			//"Password must have 1 Uppercase, 1 Lowercase, 1 number, 1 non-alphanumeric, and at least 6 characters.")]
			services.AddIdentity<AppUser, IdentityRole>(options => //allow DI for Services and configuration 
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;


			}).AddEntityFrameworkStores<PrintMaticContext>()
			  .AddDefaultTokenProviders();

            services.AddAuthentication(/*JwtBearerDefaults.AuthenticationScheme*/ Options =>
            {
                Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["JWT:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["JWT:Audience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])),
                        ValidateLifetime = true,
                    };
                });

            return services;
        }
    }
}
