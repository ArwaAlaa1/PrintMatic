using Microsoft.EntityFrameworkCore;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.Core.Repository.Contract;
using PrintMatic.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Repository.Repository
{
    public class AddressRepository : GenericRepository<Address> ,IAddressRepository 
    {
      
        public AddressRepository(PrintMaticContext printMaticContext)
            :base(printMaticContext)
        {
            
        }
        public async Task<IEnumerable<Address>> GetAllUserAddress(string id)
       {
           var addresses=await  _context.Address.Where( a=>a.AppUserId== id).ToListAsync();
            return addresses ?? new List<Address>()  ;
        }

		public async Task<Address?> GetUserAddress(int id)
		{
            var address=await _context.Address.Where(a => a.Id == id).Include(u => u.AppUser).FirstOrDefaultAsync();
            //address.AppUserId = address.AppUser.Id;
            return address;
        }
	}
}
