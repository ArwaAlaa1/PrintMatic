using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using PrintMatic.Core;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.Core.Repository.Contract;
using PrintMatic.Extensions;

using PrintMatic.Extentions;

using PrintMatic.Helper;
using PrintMatic.Repository;

using PrintMatic.Repository.Data;

using PrintMatic.Repository.Repository;
using PrintMatic.Services;
using StackExchange.Redis;


namespace PrintMatic
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);


			#region Service Container
			// Add services to the container.
			builder.Services.AddScoped<IAuthorizationMiddlewareResultHandler, ValidationAuthorization>();
			builder.Services.AddControllers();

			//Add Context Services
			builder.Services.AddDbContext<PrintMaticContext>(
				options => options.UseSqlServer(builder.Configuration.GetConnectionString("conn")));
            builder.Services.AddScoped(typeof(ICategoryRepository), typeof(CategoryRepository));
            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            builder.Services.AddAutoMapper(typeof(MappingProfiles));
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped(typeof(IProductSale), typeof(ProductSaleRepository));
            builder.Services.AddScoped(typeof(IProductPhoto), typeof(ProductPhotoRepository));
            builder.Services.AddAutoMapper(typeof(MappingProfiles));
            builder.Services.AddScoped(typeof(IProdduct), typeof(ProductRepository));
            builder.Services.AddScoped(typeof(IProductColor), typeof(ProductColorRepository));
            builder.Services.AddScoped(typeof(IProductSize), typeof(ProductSizeRepository));
            builder.Services.AddScoped(typeof(IReviewRepository), typeof(ReviewRepository));

            builder.Services.AddSingleton<IConnectionMultiplexer>((serverprovider) =>
			{
				var connectionredis = builder.Configuration.GetConnectionString("Redis");
				return ConnectionMultiplexer.Connect(connectionredis);
			});

			builder.Services.AddApplicationServices();

			builder.Services.AddIdentityServices(builder.Configuration);
			builder.Services.AddAuthorization();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = context =>
				{
					// Extract and concatenate all error messages into one
					var errorMessage = string.Join(", ", context.ModelState.Values
						.SelectMany(v => v.Errors)
						.Select(e => e.ErrorMessage));

					return new BadRequestObjectResult(new { Message = errorMessage });
				};
			});
	builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAllOrigins",
					builder =>
					{
						builder.AllowAnyOrigin()
							   .AllowAnyMethod()
							   .AllowAnyHeader();
					});
			});


			
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			#endregion



			var app = builder.Build();

			#region Update Database

			using var scope = app.Services.CreateScope();

			var services = scope.ServiceProvider;


			var loggerfactory = services.GetRequiredService<ILoggerFactory>();
			try
			{
				var dbcontext = services.GetRequiredService<PrintMaticContext>();
				//Ask Clr for creating object from dbcontext Explicitly

				await dbcontext.Database.MigrateAsync();

				var usermanager = services.GetRequiredService<UserManager<AppUser>>();
				var rolemanager = services.GetRequiredService<RoleManager<IdentityRole>>();
				await AppSeeding.SeedUsersAsync(usermanager,rolemanager);

			}
			catch (Exception ex)
			{
				var logger = loggerfactory.CreateLogger(typeof(Program));
				logger.LogError(ex, "An Error Occured During Apply Migration ");

			}

			#endregion

			app.UseSwagger();
			if (app.Environment.IsDevelopment())
				app.UseSwaggerUI();
			else
				app.UseSwaggerUI(options =>
				{
					options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
					options.RoutePrefix = string.Empty;
				});
			//app.UseSwagger();
			//app.UseSwaggerUI();
			//app.Use
			//if (app.Environment.IsDevelopment())
			//{
			//	app.UseSwagger();
			//	app.UseSwaggerUI();
			//}Swagger();
			//app.UseSwagger();
			//app.UseSwaggerUI(c =>
			//{
			//	c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
			//	c.RoutePrefix = string.Empty; // Makes Swagger UI the app's root
			//});


			app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS
			app.UseStaticFiles(); // Serve static files

			app.UseRouting(); // Configure routing

			app.UseCors("AllowAllOrigins"); // Apply CORS policy

			app.UseAuthentication(); // Enable authentication
			app.UseAuthorization(); // Enable authorization

			

			app.MapControllers();      // 7. Map routes

			app.Run();                 // 8. Run the application

			
		}
	}
}