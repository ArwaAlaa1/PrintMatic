using PrintMatic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrintMatic.Core.Repository.Contract
{
    public interface IFavouriteRepository : IGenericRepository<Favorite>
    {
        Task<Favorite> GetFavoriteAsync(int ProductId , string UserId);
    }
}
