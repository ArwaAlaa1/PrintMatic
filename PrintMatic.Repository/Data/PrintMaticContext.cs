﻿using Microsoft.EntityFrameworkCore;
using PrintMatic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Repository.Data
{
	public class PrintMaticContext: DbContext
	{
        public PrintMaticContext()
        {
            
        }
        public PrintMaticContext(DbContextOptions<PrintMaticContext> options)
            :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Sale> Sales { get; set; }

        public DbSet<Review> Reviews { get; set; }
    }
}
