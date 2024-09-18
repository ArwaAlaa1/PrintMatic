using PrintMatic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Repository.Contract
{
    public interface ICategoryRepository: IGenericRepository<Category>
    {
        Task<Category> GetCategoryWithProduct(int id);
    }
}
