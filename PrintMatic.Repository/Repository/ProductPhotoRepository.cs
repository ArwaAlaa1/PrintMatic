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
    public class ProductPhotoRepository : IProductPhoto
    {
        private readonly PrintMaticContext _context;

        public ProductPhotoRepository(PrintMaticContext context)
        {
            _context = context;
        }
        public void Add(ProductPhotos entity)
        {
            _context.Add(entity);
        }

        public async Task<IEnumerable<ProductPhotos>> GetAllAsync()
        {
            return await _context.Set<ProductPhotos>().Where(x => x.IsDeleted == false).Include("Product").ToListAsync();
        }

        public async Task<ProductPhotos> GetByIDAsync(int ProductId , string Photo)
        {
            return await _context.Set<ProductPhotos>().Include("Product").Where(x => x.ProductId == ProductId && x.Photo == Photo).FirstOrDefaultAsync();
        }
        public void Update(ProductPhotos entity)
        {
            _context.Update(entity);
        }
        public void Delete(ProductPhotos entity)
        {
            _context.Remove(entity);
        }
        public int Complet()
        {
           return _context.SaveChanges();
        }
    }
}
