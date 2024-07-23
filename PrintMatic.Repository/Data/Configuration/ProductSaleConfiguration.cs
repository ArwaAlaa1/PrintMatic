using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrintMatic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Repository.Data.Configuration
{
    public class ProductSaleConfiguration : IEntityTypeConfiguration<ProductSale>
    {
        public void Configure(EntityTypeBuilder<ProductSale> builder)
        {
            builder.HasKey(c => new { c.ProductId, c.SaleId });
        }
    }
}
