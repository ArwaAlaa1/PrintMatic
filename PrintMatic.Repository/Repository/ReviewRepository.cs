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
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        private readonly PrintMaticContext _context;

        public ReviewRepository(PrintMaticContext context):base(context) 
        {
            _context = context;
        }
        public async Task<IEnumerable<Review>> GetAllIncludeProductAsync()
        {
            return await _context.Reviews.Include("Product").Where(x => x.IsDeleted == false).ToListAsync();
        }
        

        
        public async Task<Review> GetIdIncludeProductAsync(int id)
        {
            return  _context.Reviews.Include("Product").Where(x => x.Id == id).FirstOrDefault();
        }
        public async Task<IEnumerable<Review>> GetReviewsOfPro(int ProductId)
        {
            return await _context.Reviews.Where(x => x.ProductId == ProductId && x.IsDeleted == false).ToListAsync();
        }

    }
}
