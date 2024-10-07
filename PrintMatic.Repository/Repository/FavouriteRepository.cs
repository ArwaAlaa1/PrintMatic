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
    public class FavouriteRepository : GenericRepository<Favorite>, IFavouriteRepository
    {
        private readonly PrintMaticContext _context;

        public FavouriteRepository(PrintMaticContext context):base(context) 
        {
            _context = context;
        }
        
        public async Task<Favorite> GetFavoriteAsync(int ProductId, string UserId)
        {
            return await _context.Favorites.Where(x => x.IsDeleted == false && x.Product.IsDeleted == false && x.Product.Enter == true && x.ProductId == ProductId && x.UserId == UserId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Favorite>> GetFavorites(string UserId)
        {
            return await _context.Favorites.Include("Product").Where(x => x.IsDeleted == false && x.Product.IsDeleted== false && x.Product.Enter == true && x.UserId == UserId).ToListAsync();
        }
    }
}
