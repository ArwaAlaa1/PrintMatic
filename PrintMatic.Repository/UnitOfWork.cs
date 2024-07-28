using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Repository.Data;
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


        public UnitOfWork(PrintMaticContext Context)
        {
            _Context = Context;
            generic = new GenericRepository<T>(_Context);
        }

        public int Complet()
        {
           return _Context.SaveChanges();
        }

        public void Dispose()
        {
             _Context.Dispose();
        }
    }
}
