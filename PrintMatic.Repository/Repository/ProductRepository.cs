using FuzzySharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PrintMatic.Core;
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


        public ProductRepository(PrintMaticContext context) :base(context) 
        {
            _context = context;
          
        }
        public async Task<Product?> GetIDProducts(int id)
        {
            return _context.Products.Include("Category").Include("AppUser").Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefault();
        }
        public async Task<IEnumerable<Product>> GetWaitingProducts()
        {
            return await _context.Products.Include("Category").Include("AppUser").Where(x => x.Enter == false && x.IsDeleted == false).ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products.Include("Category").Include("AppUser").Where(x => x.Enter == true && x.IsDeleted == false).ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetAllProductswithouttables()
        {
            return await _context.Products.Where(x => x.Enter == true && x.IsDeleted == false).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetYourProducts(string userName)
        {
            return await _context.Products.Include("Category").Include("AppUser").Where(x => x.AppUser.UserName == userName&& x.Enter == true && x.IsDeleted == false).ToListAsync();

        }
        public async Task<IEnumerable<Product>> GetInActiveProducts()
        {
            return await _context.Products.Include("Category").Include("AppUser").Where(x=> x.IsDeleted == true).ToListAsync();

        }
        public async Task<IEnumerable<Product>> GetUserWithHisProducts(string id)
        {
            return await _context.Products.Include("AppUser").Where(x => x.AppUser.Id == id && x.IsDeleted == false && x.Enter == true).ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchByName(string ProName)
        {
            var products = _context.Products.Where(x => x.IsDeleted == false && x.Enter == true).AsEnumerable();
            int similarityThreshold = 70;

            var listofProducts = products.Where(x => Fuzz.Ratio(x.Name, ProName) > similarityThreshold || x.Name.Trim().ToLower().Contains(ProName.Trim().ToUpper())).ToList();
           
                return listofProducts;
            
            
        }
    }
}
