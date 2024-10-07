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
	public class ShippingCostConfiguration : IEntityTypeConfiguration<ShippingCost>
	{
		

		public void Configure(EntityTypeBuilder<ShippingCost> builder)
		{
			builder.Property(s => s.Cost).HasColumnType("decimal(18,2)");
		}
	}
}
