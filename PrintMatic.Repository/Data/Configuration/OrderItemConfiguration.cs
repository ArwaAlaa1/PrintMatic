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
			builder.OwnsOne(OItem => OItem.ProductItem, ProductItem =>
			{
				ProductItem.Property(o => o.ItemType).HasConversion(
			 IStatus => IStatus.ToString(), //store
			 IStatus => (ItemType)Enum.Parse(typeof(ItemType), IStatus)//retrive
			 );
				ProductItem.WithOwner();
				ProductItem.Property(p => p.Photos)
            .HasColumnType("NVARCHAR(MAX)");
            });
            
            builder.Property(o => o.TotalPrice).HasColumnType("decimal(18,2)");
			builder.Property(o => o.OrderItemStatus).HasConversion(
			OStatus => OStatus.ToString(), //store
			OStatus => (OrderItemStatus)Enum.Parse(typeof(OrderItemStatus), OStatus)//retrive
			);
			//builder.Property(o => o.ProductOrderDetails.NormalPrice).HasColumnType("decimal(18,2)");
			//builder.Property(o => o.ProductOrderDetails.PriceAfterSale).HasColumnType("decimal(18,2)");
		}
	}
}
