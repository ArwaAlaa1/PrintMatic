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
    public class ProductColorRepository : GenericRepository<ProductColor>, IProductColor
    {
        private readonly PrintMaticContext _context;

        public ProductColorRepository(PrintMaticContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ProductColor>> GetIdOfProAsync(int id)
        {
            return await _context.ProductColors.Where(x => x.ProductId == id).ToListAsync();
        }
        public async Task<ProductColor?> GetColor_Pro(int id)
        {
            return _context.ProductColors.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefault();
        }
       
        public void Delete(int id)
        {
            var item = _context.ProductColors.Where(x=> x.Id == id).FirstOrDefault();
            _context.ProductColors.Remove(item);
        }
    }
}
