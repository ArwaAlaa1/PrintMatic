using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
           // optionsBuilder.UseSqlServer("Data source = .; Database = PtintMaticDB ; integrated security = true ;MultipleActiveResultSets=true;TrustServerCertificate=True");

		    optionsBuilder.UseSqlServer(configuration.GetConnectionString("conn"));
		}
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
    }
}
