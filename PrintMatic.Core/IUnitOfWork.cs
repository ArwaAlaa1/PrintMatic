using PrintMatic.Core.Entities;
using PrintMatic.Core.Repository.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core
{
	public interface IUnitOfWork<T> : IDisposable where T : BaseEntity
	{
		public IGenericRepository<T> generic { get; set; }

		public IReviewRepository review { get; set; }

		public IProdduct prodduct { get; set; }
        public ICategoryRepository category { get; set; }
        public IProductColor color { get; set; }
        public IProductSize size { get; set; }
        public ISaleRepository Sale { get; set; }
        int Complet();
        Task<int> CompletAsync();
    }
}
