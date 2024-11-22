using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Repository.Data.Configuration
{
	public class OrderConfiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.OwnsOne(o => o.ShippingAddress, ShippingAddress => ShippingAddress.WithOwner());
			builder.Property(o => o.Status).HasConversion(
				OStatus => OStatus.ToString(), //store
				OStatus =>(OrderStatus) Enum.Parse(typeof(OrderStatus), OStatus)//retrive
				);
            builder.Property(o => o.StatusReady).HasConversion(
                OStatus => OStatus.ToString(), //store
                OStatus => (OrderReady)Enum.Parse(typeof(OrderReady), OStatus)//retrive
                );

            builder.Property(o => o.SubTotal).HasColumnType("decimal(18,2)");

		}
	}
}
