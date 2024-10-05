using Microsoft.Extensions.DependencyInjection;
using PrintMatic.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Repository.Contract
{
    public interface IAddressRepository
    {
        public Task<IEnumerable<Address>> GetAllUserAddress(string id);
        public Task<Address> GetUserAddress(int id);
    }
}
