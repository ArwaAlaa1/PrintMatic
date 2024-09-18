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
    public class CategoryRepository: GenericRepository<Category> ,ICategoryRepository
    {
        private readonly PrintMaticContext _context;

        public CategoryRepository(PrintMaticContext context):base(context)
        {
            _context = context;
        }

        public async Task<Category> GetCategoryWithProduct(int id)
        {
           return await _context.Categories.Include(x => x.Products.Where(x => x.IsDeleted == false && x.Enter== true)).ThenInclude(x => x.ProductPhotos).Include(x => x.Products.Where(x => x.IsDeleted == false && x.Enter == true)).ThenInclude(x => x.ProductSales).FirstAsync(x => x.Id == id);
        }
    }
}
