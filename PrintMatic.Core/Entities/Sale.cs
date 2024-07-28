﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Entities
{
    public class Sale : BaseEntity
    {
        public bool IsOnSale { get; set; }
        public decimal PriceAfterSale { get; set; }
        public DateTime SaleStartDate { get; set; }
        public DateTime SaleEndDate { get; set; }
        [DisplayName("Discount")]
        public int SaleDiscountPercentage { get; set; }


		

	}
}
