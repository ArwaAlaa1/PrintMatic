using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrintMatic.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Repository.Data.Configuration
{
	public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
	{
		public void Configure(EntityTypeBuilder<OrderItem> builder)
		{
			builder.OwnsOne(OItem => OItem.ProductItem, ProductItem => ProductItem.WithOwner());
			builder.Property(o => o.Price).HasColumnType("decimal(18,2)");
			//builder.Property(o => o.ProductOrderDetails.NormalPrice).HasColumnType("decimal(18,2)");
			//builder.Property(o => o.ProductOrderDetails.PriceAfterSale).HasColumnType("decimal(18,2)");
		}
	}
}
