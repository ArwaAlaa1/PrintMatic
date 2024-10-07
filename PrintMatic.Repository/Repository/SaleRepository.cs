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
    public class SaleRepository : GenericRepository<Sale>, ISaleRepository
    {
        private readonly PrintMaticContext _context;

        public SaleRepository(PrintMaticContext context):base(context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<Sale>> GetActiveSales()
        {
            return await _context.Sales.Where(x => x.IsDeleted == false && x.SaleStartDate < DateTime.Now && x.SaleEndDate > DateTime.Now).ToListAsync();
        }
    }
}
