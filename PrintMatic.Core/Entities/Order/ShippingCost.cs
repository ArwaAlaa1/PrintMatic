using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Entities.Order
{
    public class ShippingCost : BaseEntity
    {
        public string City { get; set; }
        public decimal Cost { get; set; }
        public string ShippingTime { get; set; }

    }
}
