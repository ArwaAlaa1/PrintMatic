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
            return await _context.Set<ProductPhotos>().Where( x => x.Enter == true && x.IsDeleted == false).Include("Product").ToListAsync();
        }

        public async Task<ProductPhotos> GetByIDAsync(int ProductId , string Photo)
        {
            return await _context.Set<ProductPhotos>().Include("Product").Where(x => x.ProductId == ProductId && x.Photo == Photo).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProductPhotos>> GetYourProductPhotos(string userName)
        {
            return await _context.ProductPhotos.Include("Product").Where(x => x.Product.AppUser.UserName == userName && x.Enter == true && x.IsDeleted == false).ToListAsync();

        }
        public async Task<IEnumerable<ProductPhotos>> GetWaitingProducts()
        {
            return await _context.ProductPhotos.Include("Product").Where(x => x.Enter == false && x.IsDeleted == false).ToListAsync();
        }
        public async Task<IEnumerable<ProductPhotos>> GetPhotosOfProduct(int id)
        {
            return await _context.ProductPhotos.Include("Product").Where(x =>x.ProductId == id  && x.IsDeleted == false).ToListAsync();
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
           return  _context.SaveChanges();
        }

    }
}
