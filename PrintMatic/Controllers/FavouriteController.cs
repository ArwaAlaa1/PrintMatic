using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.Core.Repository.Contract;
using PrintMatic.DTOS;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PrintMatic.Controllers
{
    [Authorize]
    public class FavouriteController : BaseApiController
    {
        private readonly IFavouriteRepository _fav;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProdduct _product;
        private readonly UserManager<AppUser> _user;
        private readonly IMapper _mapper;
        private readonly IProductPhoto _productPhoto;
        private readonly IProductSale _productSale;
        private readonly IReviewRepository _review;
        private readonly IProductColor _color;

        public FavouriteController(IFavouriteRepository fav, IUnitOfWork unitOfWork, IProdduct product
            , UserManager<AppUser> user, IMapper mapper, IProductPhoto productPhoto, IProductSale productSale, IReviewRepository review
            , IProductColor color)
        {
            _fav = fav;
            _unitOfWork = unitOfWork;
            _product = product;
            _user = user;
            _mapper = mapper;
            _productPhoto = productPhoto;
            _productSale = productSale;
            _review = review;
            _color = color;
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
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productId);
                if (product == null)
                {
                    return BadRequest("لا يوجد منتج بهذا الكود");
                }
                var Fav = await _fav.GetFavoriteAsync(productId, user.Id);
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
                    _unitOfWork.Repository<Favorite>().Add(favorite);
                    var count = await _unitOfWork.Complet();
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
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productId);
                if (product == null)
                {
                    return BadRequest("لا يوجد منتج بهذا الكود");
                }
                var Fav = await _fav.GetFavoriteAsync(productId, user.Id);
                if (Fav != null)
                {
                    _unitOfWork.Repository<Favorite>().Delete(Fav);
                    var count = await _unitOfWork.Complet();
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
                    Message = $"فشلت عملية حذف المنتج من المفضله {ex.Message.ToString() ?? ex.InnerException?.Message.ToString()}"
                });
            }

        }

        [HttpGet("GetAllFavorites")]
        public async Task<IActionResult> GetAll(string? token)
        {
            try
            {
                List<ProductDto> productDtos = new List<ProductDto>();
                var user = await _user.GetUserAsync(User);
                if (user == null)
                {
                    return BadRequest("لا يوجد مستخدم بهذا الكود");
                }
                var list = await _fav.GetFavorites(user.Id);
                var FavListMapped = _mapper.Map<IEnumerable<Favorite>, IEnumerable<FavouriteDto>>(list);
                if (FavListMapped.Any())
                {
                    foreach (var favourite in FavListMapped)
                    {

                        var product = await _product.Get(favourite.Product.Id);
                        var products = await ProductDto.GetProducts(favourite.Product, product.ProductSales, product.ProductPhotos, product.Reviews, product.ProductColors);
                        string userId;
                        if (!string.IsNullOrEmpty(token))
                        {
                            var handler = new JwtSecurityTokenHandler();
                            var jwtToken = handler.ReadJwtToken(token);


                            userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
                                          ?? jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

                            if (!string.IsNullOrEmpty(userId))
                            {
                                var fav = await _fav.GetFavoriteAsync(products.Id, userId);
                                if (fav != null)
                                {
                                    products.IsFav = true;
                                }
                            }
                        }


                        productDtos.Add(products);
                    }
                }
                    return Ok(productDtos);

                
            }
            catch (Exception ex)
            {
                return BadRequest(new Response()
                {
                    Message = $"{ex.Message.ToString() ?? ex.InnerException?.Message.ToString()}"
                });
            }


        }
    }
}

