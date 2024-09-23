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
    public class ProductSizeRepository:GenericRepository<ProductSize>,IProductSize
    {
        private readonly PrintMaticContext _context;

        public ProductSizeRepository(PrintMaticContext context):base(context)
        {
            _context = context;
        }

        public async Task<List<ProductSize>> GetIdOfProAsync(int id)
        {
            return await _context.ProductSizes.Where(x => x.ProductId == id).ToListAsync();
        }
        public async Task<ProductSize?> GetSize_Pro(int id)
        {
            return _context.ProductSizes.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefault();
        }
        public void Delete(int id)
        {
            var item = _context.ProductSizes.Where(x => x.Id == id).FirstOrDefault();
            _context.ProductSizes.Remove(item);
        }
    }
}
