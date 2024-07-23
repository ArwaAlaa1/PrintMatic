using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Entities
{

    public class ProductSale
    {
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public Sale Sale { get; set; }
        public int SaleId { get; set; }
    }
}
