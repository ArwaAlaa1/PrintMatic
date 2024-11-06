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
        public async Task<IEnumerable<Product>> FilterSearsh(string? ProName , int? CategoryId , string? HexCode , decimal? price , string? size )
        {
        
                var products =  _context.Products.Where(x => x.IsDeleted == false && x.Enter == true).AsEnumerable();
                int similarityThreshold = 70;
                if (!string.IsNullOrWhiteSpace(ProName))
                {
                     products = products.Where(x => Fuzz.Ratio(x.Name, ProName) > similarityThreshold || x.Name.Trim().ToLower().Contains(ProName.Trim().ToUpper())).ToList();
                }
                if (CategoryId.HasValue)
                {
                    products = products.Where(x =>x.CategoryId == CategoryId).ToList();
                }
                if (!string.IsNullOrWhiteSpace(HexCode))
                {
                    List<ProductColor> ProColor = new List<ProductColor>();
                List<Product> products1 = new List<Product>();
                foreach (var pro in products)
                    {
                       var product = _context.ProductColors.Where(x => x.IsDeleted == false && x.ProductId == pro.Id && x.HexCode == HexCode).FirstOrDefault();
                    if (product != null) { 
                        ProColor.Add(product);
                    }
                    }
                foreach (var item in ProColor)
                {
                    
                   var  pro = products.Where(x => x.Id == item.ProductId).FirstOrDefault();
                    if (pro != null)
                    {
                        products1.Add(pro);
                    }
                }
                products = products1;
                }
                
                if(price.HasValue)
                {
                    products = products.Where(x => x.TotalPrice == price).ToList();
                }
            if (!string.IsNullOrWhiteSpace(size))
            {
                List<ProductSize> ProSize = new List<ProductSize>();
                List<Product> products1 = new List<Product>();
                foreach (var pro in products)
                {
                    var product = _context.ProductSizes.Where(x => x.IsDeleted == false && x.ProductId == pro.Id && x.Size == size).FirstOrDefault();
                    if (product != null)
                    {
                        ProSize.Add(product);
                    }
                }
                foreach (var item in ProSize)
                {
                    var pro = products.Where(x => x.Id == item.ProductId).FirstOrDefault();
                    if (pro != null)
                    {
                        products1.Add(pro);
                    }
                }
                products = products1;
            }


            return products;
            }

            

        }
  }

