using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrintMartic_DashBoard.Helper;
using PrintMatic.Core;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.Repository;
using PrintMatic.Repository.Data;

namespace PrintMartic_DashBoard
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

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
			
			#endregion

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}
