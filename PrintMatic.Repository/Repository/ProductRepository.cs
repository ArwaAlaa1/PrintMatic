﻿using Microsoft.EntityFrameworkCore;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Repository.Contract;
using PrintMatic.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Repository.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProdduct
    {
        private readonly PrintMaticContext _context;

        public ProductRepository(PrintMaticContext context):base(context) 
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetWaitingProducts()
        {
            return await _context.Products.Where(x => x.Enter == false).ToListAsync();
        }
    }
}