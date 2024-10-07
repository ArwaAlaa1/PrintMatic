using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.DTOS;

namespace PrintMatic.Controllers
{
    [Authorize]
    public class FavouriteController : BaseApiController
    {
        private readonly IUnitOfWork<Favorite> _fav;
        private readonly IUnitOfWork<Product> _product;
        private readonly UserManager<AppUser> _user;
        private readonly IMapper _mapper;

        public FavouriteController(IUnitOfWork<Favorite> fav , IUnitOfWork<Product> product
            ,UserManager<AppUser> user , IMapper mapper)
        {
            _fav = fav;
            _product = product;
            _user = user;
            _mapper = mapper;
        }
        [HttpPost("AddFavourite")]
        public async Task<IActionResult> AddFavourite(int productId)
        {
            try
            {
                var user = await _user.GetUserAsync(User);
                if (user == null)
                {
                    return BadRequest("لا يوجد مستخدم بهذا الكود");
                }
                var product = await _product.generic.GetByIdAsync(productId);
                if (product == null)
                {
                    return BadRequest("لا يوجد منتج بهذا الكود");
                }
                var Fav = await _fav.Fav.GetFavoriteAsync(productId, user.Id);
                if (Fav != null)
                {
                    return BadRequest("هذا المنتج مضاف الى المفضله بالفعل");
                }
                else
                {
                    Favorite favorite = new Favorite()
                    {
                        ProductId = productId,
                        UserId = user.Id
                    };
                    _fav.generic.Add(favorite);
                    var count = _fav.Complet();
                    if (count > 0)
                    {
                        return Ok(new Response()
                        {
                            Message = $"تم إضافة المنتج إلى المفضلة بنجاح"
                        });
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(new Response()
                {
                    Message = $"فشلت عملية الإضافه {ex.Message.ToString() ?? ex.InnerException?.Message.ToString()}"
                });
            }
           
        }


        [HttpPost("DeleteFavourite")]
        public async Task<IActionResult> DeleteFavourite(int productId)
        {
            try
            {
                var user = await _user.GetUserAsync(User);
                if (user == null)
                {
                    return BadRequest("لا يوجد مستخدم بهذا الكود");
                }
                var product = await _product.generic.GetByIdAsync(productId);
                if (product == null)
                {
                    return BadRequest("لا يوجد منتج بهذا الكود");
                }
                var Fav = await _fav.Fav.GetFavoriteAsync(productId, user.Id);
                if (Fav != null)
                {
                    _fav.generic.Delete(Fav);
                    var count = _fav.Complet();
                    if (count > 0)
                    {
                        return Ok(new Response()
                        {
                            Message = $"تم حذف المنتج من المفضله بنجاح"
                        });
                    }
                }
                return BadRequest("هذا المنتج غير موجود في المفضله");
            }
            catch (Exception ex)
            {
                return BadRequest(new Response()
                {
                    Message = $"فشلت عملية حذف المنتج من المفضله"
                });
            }

        }

        //[HttpGet("GetAllFavorites")]
        //public async Task<IActionResult> GetAll()
        //{
        //    var list = await _fav.
        //}
    }
}
