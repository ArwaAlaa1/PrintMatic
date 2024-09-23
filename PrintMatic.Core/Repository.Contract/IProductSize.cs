using PrintMatic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Repository.Contract
{
    public interface IProductSize : IGenericRepository<ProductSize>
    {
        Task<List<ProductSize>> GetIdOfProAsync(int id);
        Task<ProductSize?> GetSize_Pro(int id);

        void Delete(int id);
    }
}
