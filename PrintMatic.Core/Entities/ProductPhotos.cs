﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Entities
{
    public class ProductPhotos
    {
        public string Photo {  get; set; }

        public Product Product { get; set; }

        public int ProductId { get; set; }
    }
}