using PrintMatic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Repository.Contract
{
    public interface IProductColor: IGenericRepository<ProductColor>
    {
        Task<List<ProductColor>> GetIdOfProAsync(int id);

        Task<ProductColor?> GetColor_Pro(int Id);
        void Delete(int id);
    }
}
