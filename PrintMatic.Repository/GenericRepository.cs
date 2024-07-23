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
	public class GenericRepository<T>: IGenericRepository<T> where T : BaseEntity
    {
        private readonly PrintMaticContext _context;

        public GenericRepository(PrintMaticContext context)
        {
            _context = context;
        }
    }
}
