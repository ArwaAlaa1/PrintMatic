using Microsoft.EntityFrameworkCore;
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
    public class ProductSaleRepository : IProductSale
    {
        private readonly PrintMaticContext _context;

        public ProductSaleRepository(PrintMaticContext context) 
        {
            _context = context;
        }

        public void Add(ProductSale entity)
        {
            _context.productSales.Add(entity);
        }

        public int Complet()
        {
           return _context.SaveChanges();
        }

        public async Task<IEnumerable<ProductSale>> GetAllAsync()
        {
            return await _context.productSales.Include("Product").Include("Sale").Where(x => x.IsDeleted == false).ToListAsync();
        }

        public async Task<ProductSale> GetByIDAsync(int ProductId, int SaleId)
        {
            return await _context.productSales.FindAsync(ProductId , SaleId);
        }

        public void Update(ProductSale entity)
        {
            _context.Update(entity);
        }
        public void Delete(ProductSale productSale)
        {
            _context.Remove(productSale);
        }
    }
}
