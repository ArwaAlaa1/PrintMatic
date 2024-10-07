using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.Core.Entities.Order;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Address = PrintMatic.Core.Entities.Identity.Address;
using Order = PrintMatic.Core.Entities.Order.Order;

namespace PrintMatic.Repository.Data
{
	public class PrintMaticContext: IdentityDbContext<AppUser>
	{
        private readonly IConfiguration configuration;

        public PrintMaticContext(DbContextOptions<PrintMaticContext> options,IConfiguration configuration)
            :base(options)
        {
            this.configuration = configuration;
        }

        public PrintMaticContext()
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           base.OnConfiguring(optionsBuilder);
<<<<<<< HEAD
			// optionsBuilder.UseSqlServer("Server=db8617.public.databaseasp.net; Database=db8617; User Id=db8617; Password=2f!YQ3q@4#Zh; Encrypt=False; MultipleActiveResultSets=True;");
			//optionsBuilder.UseSqlServer("Data source =DESKTOP-9IISLS5; DataBase = PrintMaticDB; Trusted_Connection= True;TrustServerCertificate=True");
         optionsBuilder.UseSqlServer("server =Arwa-Alaa\\SQLEXPRESS; DataBase = PrintMatic2; Trusted_Connection = True; TrustServerCertificate = True");
			//  optionsBuilder.UseSqlServer(configuration.GetConnectionString("conn"));
		}
=======
           //optionsBuilder.UseSqlServer("server =Arwa-Alaa\\SQLEXPRESS; DataBase = PrintMatic; Trusted_Connection= True;TrustServerCertificate=True");
            optionsBuilder.UseSqlServer("Data source =DESKTOP-9IISLS5; DataBase = PrintMaticDB; Trusted_Connection= True;TrustServerCertificate=True");

            //  optionsBuilder.UseSqlServer(configuration.GetConnectionString("conn"));
        }
>>>>>>> e7a1ce2fcefa8289c004219ba33f26f0fd9a5f0d
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<ProductPhotos> ProductPhotos { get; set; }
        public DbSet<ProductSale> productSales { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public  DbSet<ShippingCost> ShippingCosts { get; set; }

    }
}
