using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Repository.Contract;
using PrintMatic.Repository.Data;
using PrintMatic.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Repository
{
	public class UnitOfWork<T> : IUnitOfWork<T> where T : BaseEntity
    {
        public IGenericRepository<T> generic { get; set; }
        private readonly PrintMaticContext _Context;
        public IReviewRepository review {  get; set; }
        public IProdduct prodduct { get; set; }

        public ICategoryRepository category { get; set; }
        public IProductColor color { get; set; }
        public IProductSize size { get; set; }
        public ISaleRepository Sale { get; set; }

        public UnitOfWork(PrintMaticContext Context)
        {
            _Context = Context;
            generic = new GenericRepository<T>(_Context);
            review = new ReviewRepository(_Context);
            prodduct = new ProductRepository(_Context);
            category = new CategoryRepository(_Context);
            color = new ProductColorRepository(_Context); 
            size = new ProductSizeRepository(_Context);
            Sale = new SaleRepository(_Context);
        }

        public int Complet()
        {
           return _Context.SaveChanges();
        }
        public async Task<int> CompletAsync()
        {
            return await _Context.SaveChangesAsync();
        }
        public void Dispose()
        {
             _Context.Dispose();
        }
    }
}
