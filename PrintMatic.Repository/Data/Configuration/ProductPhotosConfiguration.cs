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
    public class ProductPhotosConfiguration : IEntityTypeConfiguration<ProductPhotos>
    {
        public void Configure(EntityTypeBuilder<ProductPhotos> builder)
        {
            builder.HasKey(x => new { x.ProductId, x.Photo });
        }
    }
}
