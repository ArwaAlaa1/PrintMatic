using PrintMatic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Repository.Contract
{
    public interface IProdduct
    {
        Task<Product> GetIDProducts(int id);
        Task<IEnumerable<Product>> GetWaitingProducts();
        Task<IEnumerable<Product>> GetAllProducts();
        Task<IEnumerable<Product>> GetYourProducts(string userName);
        Task<IEnumerable<Product>> GetInActiveProducts();
        Task<IEnumerable<Product>> GetUserWithHisProducts(string id);
        Task<IEnumerable<Product>> SearchByName(string ProName);
        Task<IEnumerable<Product>> GetAllProductswithouttables();
        Task<Product> GetProductWithPhotos(int id);
    }
}
