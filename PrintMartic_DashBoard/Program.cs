using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using PrintMartic_DashBoard.Helper;
using PrintMatic.Core;

using PrintMatic.Core.Entities.Identity;

using PrintMatic.Core.Repository.Contract;

using PrintMatic.Repository;
using PrintMatic.Repository.Data;
using PrintMatic.Repository.Repository;
using System.Globalization;

namespace PrintMartic_DashBoard
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);


			//#region language services
		//	builder.Services.AddControllersWithViews()
		//.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
		//.AddDataAnnotationsLocalization();

		//	builder.Services.Configure<RequestLocalizationOptions>(options =>
		//	{
		//		var supportedCultures = new[]
		//		{
		//	new CultureInfo("en"),
		//	new CultureInfo("ar-EG")
		//};

		//		options.DefaultRequestCulture = new RequestCulture("ar");
		//		options.SupportedCultures = supportedCultures;
		//		options.SupportedUICultures = supportedCultures;

		//		options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider()); // Allows culture to be set via query string
		//	});
		//	#endregion

			builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie(options =>
       {
           options.LoginPath = "/Account/Login"; // Redirect to this path if not authenticated
           options.AccessDeniedPath = "/Account/AccessDenied"; // Redirect here if the user is authenticated but not authorized
           options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
           options.SlidingExpiration = true;
       });

            builder.Services.AddAuthorization();


            // Add services to the container.
            builder.Services.AddControllersWithViews();
			//Add Context Services

			#region Services

			builder.Services.AddDbContext<PrintMaticContext>(

				options => options.UseSqlServer(builder.Configuration.GetConnectionString("conn")));

			builder.Services.AddIdentity<AppUser, IdentityRole>()
				.AddEntityFrameworkStores<PrintMaticContext>();

			builder.Services.AddScoped(typeof(IUnitOfWork<>),typeof(UnitOfWork<>));
			builder.Services.AddAutoMapper(typeof(MappingProfiles));
			

				
			builder.Services.AddScoped(typeof(IUnitOfWork<>),typeof(UnitOfWork<>));
			builder.Services.AddScoped(typeof(IProductPhoto), typeof(ProductPhotoRepository));
            builder.Services.AddScoped(typeof(IProductSale), typeof(ProductSaleRepository));

            builder.Services.AddAutoMapper(typeof(MappingProfiles));

			#endregion

			var app = builder.Build();

			//Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}


			app.UseStaticFiles();

			app.UseRouting();
			
			
			app.UseHttpsRedirection();

		app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Account}/{action=Login}/{id?}");

			app.Run();
		}
	}
}
