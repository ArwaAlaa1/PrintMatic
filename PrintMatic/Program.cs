
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrintMatic.Core;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.Extensions;
using PrintMatic.Helper;
using PrintMatic.Repository;
using PrintMatic.Repository.Data;
using PrintMatic.Repository.Identity;
using PrintMatic.Services;


namespace PrintMatic
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);


			#region Service Container
			// Add services to the container.

			builder.Services.AddControllers();

			//Add Context Services
			builder.Services.AddDbContext<PrintMaticContext>(
				options => options.UseSqlServer(builder.Configuration.GetConnectionString("conn")));

            builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            builder.Services.AddAutoMapper(typeof(MappingProfiles));
            builder.Services.AddScoped<ITokenService, TokenService>();



			builder.Services.AddIdentityServices(builder.Configuration);
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
				await AppSeed.UserSeedAsync(usermanager);

			}
			catch (Exception ex)
			{
				var logger = loggerfactory.CreateLogger(typeof(Program));
				logger.LogError(ex, "An Error Occured During Apply Migration ");

			}


			#endregion

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
