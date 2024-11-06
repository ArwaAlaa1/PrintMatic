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

        public ProductOrderDetails(int productId, string photo, ItemType itemType, string name, decimal price, decimal? priceAfterSale, string? color, string? size, string? text, string? date, string? photos, string? filePdf)
        {
            ProductId = productId;
            Photo = photo;
            ItemType = itemType;
            Name = name;
            Price = price;
            PriceAfterSale = priceAfterSale;
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
		public ItemType ItemType { get; set; }
        public string Name { get; set; }
		
		//price beforesale
		public decimal Price { get; set; }
		public decimal? PriceAfterSale { get; set; }
		
		public string? Color { get; set; }
		public string? Size { get; set; }
		public string? Text { get; set; }
		public string? Date { get; set; }
		public string? Photos { get; set; }
		public string?  FilePdf{ get; set; }
      
		
	}
}
