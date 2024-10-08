using PrintMatic.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PrintMatic.Core.Entities.Order
{
	public class ProductOrderDetails
	{
        public ProductOrderDetails()
        {
            
        }
        public ProductOrderDetails(int productId, string photo, string type, string name, string categoryName, string userId, int? normalMinDate, int? normalMaxDate, int? urgentMinDate, int? urgentMaxDate, decimal normalPrice, decimal? priceAfterSale, string description, string? color, string? size, string? text, string? date, List<string?> photos, string? filePdf)
		{
			ProductId = productId;
			Photo = photo;
			Type = type;
			Name = name;
			CategoryName = categoryName;
			UserId = userId;
			NormalMinDate = normalMinDate;
			NormalMaxDate = normalMaxDate;
			UrgentMinDate = urgentMinDate;
			UrgentMaxDate = urgentMaxDate;
			NormalPrice = normalPrice;
			PriceAfterSale = priceAfterSale;
			Description = description;
			Color = color;
			Size = size;
			Text = text;
			Date = date;
			Photos = photos;
			FilePdf = filePdf;
		}

		public int ProductId { get; set; }
		public string Photo { get; set; }
		//normal or busy
		public string Type { get; set; }
        public string Name { get; set; }
		public string CategoryName { get; set; }
		
		public string UserId { get; set; }
		public int? NormalMinDate { get; set; } //int
		public int? NormalMaxDate { get; set; }
		public int? UrgentMinDate { get; set; }
		public int? UrgentMaxDate { get; set; }
		//price beforesale
		public decimal NormalPrice { get; set; }
		public decimal? PriceAfterSale { get; set; }
		public string Description { get; set; }
		public string? Color { get; set; }
		public string? Size { get; set; }
		public string? Text { get; set; }
		public string? Date { get; set; }
		public List<string?> Photos { get; set; }
		public string?  FilePdf{ get; set; }
      
		
	}
}
