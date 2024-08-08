using PrintMatic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Repository.Contract
{
    public interface IProductSale
    {
        Task<IEnumerable<ProductSale>> GetAllAsync();
        Task<ProductSale> GetByIDAsync(int ProductId, int SaleId);

        void Add(ProductSale entity);

        void Update(ProductSale entity);
        void Delete(ProductSale productSale);
        int Complet();
    }
}
