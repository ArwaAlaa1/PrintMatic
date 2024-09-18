using PrintMatic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Repository.Contract
{
    public interface IProductPhoto 
    {
        Task<IEnumerable<ProductPhotos>> GetAllAsync();
        Task<ProductPhotos> GetByIDAsync(int ProductId , string Photo);

        void Add(ProductPhotos entity);

        void Update(ProductPhotos entity);
        void Delete(ProductPhotos entity);
        Task<IEnumerable<ProductPhotos>> GetWaitingProducts();
        Task<IEnumerable<ProductPhotos>> GetYourProductPhotos(string userName);
        int Complet();

	}
}
