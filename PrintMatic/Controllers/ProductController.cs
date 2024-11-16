using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.Core.Repository.Contract;
using PrintMatic.DTOS;
using System.Collections.Generic;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PrintMatic.Controllers
{

    public class ProductController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductColor _color;
        private readonly IProductSize _size;
        private readonly IMapper _mapper;
        private readonly IProductPhoto _productPhoto;
        private readonly IProductSale _productSale;
        private readonly IReviewRepository _review;
        private readonly UserManager<AppUser> _user;
        private readonly IProdduct _prodduct;
        private readonly IConfiguration _configuration;
        private readonly IFavouriteRepository _favourite;

        public ProductController(IUnitOfWork unitOfWork,
            IProductColor color, IProductSize size,
            IMapper mapper, IProductPhoto productPhoto, IProductSale productSale,
            IReviewRepository review, UserManager<AppUser> user, IProdduct prodduct
            , IConfiguration configuration, IFavouriteRepository favourite)
        {
            _unitOfWork = unitOfWork;
            _color = color;
            _size = size;
            _mapper = mapper;
            _productPhoto = productPhoto;
            _productSale = productSale;
            _review = review;
            _user = user;
            _prodduct = prodduct;
            _configuration = configuration;
            _favourite = favourite;
        }

        [HttpGet("GetAllProducts")]
        public async Task<ActionResult> GetAllProducts()
        {
            var ProList = await _prodduct.GetAllProducts();
            var ProMapped = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(ProList);
            var Products = new List<ProductDto>();
            foreach (var prod in ProMapped)
            {
                var Prowithdetails = await _prodduct.Get(prod.Id);
                var product = await ProductDto.GetProducts(prod, Prowithdetails.ProductSales, Prowithdetails.ProductPhotos, Prowithdetails.Reviews, Prowithdetails.ProductColors);
                Products.Add(product);
            }
            if (Products.Any())
            {
                return Ok(Products);
            }
            return Ok(new Response
            {
                Message = $"لا يوجد منتجات"
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDetailsDTO>> GetById(int id, string? token)
        {
            try
            {

                var product = await _prodduct.GetIDProducts(id);
                if (product == null)
                {
                    return BadRequest("There is no product with this Id");
                }
                var ProMapped = _mapper.Map<Product, ProductDetailsDTO>(product);
                ProMapped.User = new UserSimpleDetails()
                {
                    Id = product.AppUser.Id,
                    FilePath = $"{_configuration["ApiBaseUrl"]}//{product.AppUser.Photo}",
                    UserName = product.AppUser.UserName
                };
                foreach (var color in product.ProductColors)
                {
                    ProMapped.Colors.Add(color.HexCode);
                }
                foreach (var size in product.ProductSizes)
                {
                    ProMapped.Sizes.Add(size.Size);
                }
                foreach (var photo in product.ProductPhotos)
                {
                    ProMapped.Photos.Add($"{_configuration["DashboardUrl"]}//Uploads//products//{photo.Photo}");
                }
                if (product.ProductSales.Any())
                {
                    var list = new List<ProductSale>();
                    foreach (var sale in product.ProductSales)
                    {
                        if (sale.Sale.SaleEndDate > DateTime.UtcNow)
                        {
                            list.Add(sale);
                        }
                    }
                    var item = list.FirstOrDefault();
                    if (item != null)
                    {
                        ProMapped.PriceAfterSale = item.PriceAfterSale;
                    }
                }
                var Reviews = await _review.GetReviewsOfPro(ProMapped.Id);
                float? Rating = 0f;
                foreach (var review in Reviews)
                {
                    if (review != null)
                    {
                        Rating += review.Rating;
                    }

                }
                ProMapped.AvgRating = Rating / 5f;
                string userId;
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token);


                    userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
                                  ?? jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

                    if (!string.IsNullOrEmpty(userId))
                    {
                        var fav = await _favourite.GetFavoriteAsync(id, userId);
                        if (fav != null)
                        {
                            ProMapped.IsFav = true;
                        }
                    }
                }
                return Ok(ProMapped);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString() ?? ex.InnerException?.Message.ToString());
            }

        }


        [HttpGet("GetUserWithHisProducts")]
        public async Task<ActionResult<UserWithPro>> GetUserWithHisProducts(string UserId, string? token)
        {
            var user = await _user.FindByIdAsync(UserId);
            if (user == null)
            {
                return BadRequest("There is no User with this Id");
            }
            var ProList = await _prodduct.GetUserWithHisProducts(UserId);
            var proMapped = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(ProList);
            foreach (var prod in proMapped)
            {
                var product = await _prodduct.Get(prod.Id);
                var products = await ProductDto.GetProducts(prod, product.ProductSales, product.ProductPhotos, product.Reviews, product.ProductColors);
                proMapped.ToList().Add(products);
                string userId;
                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(token);


                    userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
                                  ?? jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

                    if (!string.IsNullOrEmpty(userId))
                    {
                        var fav = await _favourite.GetFavoriteAsync(prod.Id, userId);
                        if (fav != null)
                        {
                            prod.IsFav = true;
                        }
                    }
                }
            }
            return Ok(new UserWithPro()
            {
                Id = user.Id,
                UserName = user.UserName,
                FilePath = $"{_configuration["ApiBaseUrl"]}/{user.Photo}",
                ProductsCount = ProList.ToList().Count,
                products = proMapped
            });
        }
        [HttpGet("SearchByName")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> SearchByName(string? ProductName, string? token)
        {
            try
            {
                IEnumerable<ProductDto> proMapped = new List<ProductDto>();
                if (!string.IsNullOrEmpty(ProductName))
                {
                    var list = await _prodduct.SearchByName(ProductName);
                    if (list.Any())
                    {
                        proMapped = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(list);
                        foreach (var prod in proMapped)
                        {
                            var product = await _prodduct.Get(prod.Id);
                            var products = await ProductDto.GetProducts(prod, product.ProductSales, product.ProductPhotos, product.Reviews, product.ProductColors);
                            proMapped.ToList().Add(products);
                            string userId;
                            if (!string.IsNullOrEmpty(token))
                            {
                                var handler = new JwtSecurityTokenHandler();
                                var jwtToken = handler.ReadJwtToken(token);


                                userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
                                              ?? jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

                                if (!string.IsNullOrEmpty(userId))
                                {
                                    var fav = await _favourite.GetFavoriteAsync(prod.Id, userId);
                                    if (fav != null)
                                    {
                                        prod.IsFav = true;
                                    }
                                }
                            }
                        }
                    }
                }
                return Ok(proMapped);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString() ?? ex.InnerException?.Message.ToString());
            }


        }

        [HttpGet("FilterSearch")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> FilterSearch(string? ProName, int? CategoryId, string? HexCode, decimal? price, string? size, string? token)
        {
            try
            {
                IEnumerable<ProductDto> proMapped = new List<ProductDto>();

                if (!string.IsNullOrWhiteSpace(ProName) || CategoryId.HasValue || !string.IsNullOrWhiteSpace(HexCode) || !string.IsNullOrWhiteSpace(size) || price.HasValue)
                {
                    var ProList = await _prodduct.FilterSearsh(ProName, CategoryId, HexCode, price, size);
                    if (ProList.Any())
                    {
                        proMapped = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(ProList);
                        foreach (var prod in proMapped)
                        {
                            var product = await _prodduct.Get(prod.Id);
                            var products = await ProductDto.GetProducts(prod, product.ProductSales, product.ProductPhotos, product.Reviews, product.ProductColors);
                            proMapped.ToList().Add(products);
                            string userId;
                            if (!string.IsNullOrEmpty(token))
                            {
                                var handler = new JwtSecurityTokenHandler();
                                var jwtToken = handler.ReadJwtToken(token);


                                userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value
                                              ?? jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

                                if (!string.IsNullOrEmpty(userId))
                                {
                                    var fav = await _favourite.GetFavoriteAsync(prod.Id, userId);
                                    if (fav != null)
                                    {
                                        prod.IsFav = true;
                                    }
                                }
                            }
                        }
                    }
                }
                return Ok(proMapped);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString() ?? ex.InnerException?.Message.ToString());
            }

        }
    }
}
