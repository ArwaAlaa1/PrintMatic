using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Entities
{
    public class ProductSale : Entity
    {
        public virtual Product Product { get; set; }
        public int ProductId { get; set; }
        public virtual Sale Sale { get; set; }
        public int SaleId { get; set; }
        public decimal PriceAfterSale { get; set; }

    }
}
