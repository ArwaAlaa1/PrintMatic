using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.Core.Repository.Contract;
using PrintMatic.DTOS;
using System.Collections.Generic;
using System.Drawing;
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

        public ProductController(IUnitOfWork unitOfWork,
            IProductColor color, IProductSize size,
            IMapper mapper, IProductPhoto productPhoto, IProductSale productSale,
            IReviewRepository review, UserManager<AppUser> user, IProdduct prodduct
            , IConfiguration configuration)
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
        public async Task<ActionResult<ProductDetailsDTO>> GetById(int id)
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
                return Ok(ProMapped);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString() ?? ex.InnerException?.Message.ToString());
            }

        }


        [HttpGet("GetUserWithHisProducts")]
        public async Task<ActionResult<UserWithPro>> GetUserWithHisProducts(string UserId)
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
        public async Task<ActionResult<IEnumerable<ProductDto>>> SearchByName(string? ProductName)
        {
            if (!string.IsNullOrEmpty(ProductName))
            {
                var list = await _prodduct.SearchByName(ProductName);
                if (list.Any())
                {
                    var proMapped = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(list);
                    foreach (var prod in proMapped)
                    {
                        var product = await _prodduct.Get(prod.Id);
                        var products = await ProductDto.GetProducts(prod, product.ProductSales, product.ProductPhotos, product.Reviews, product.ProductColors);
                        proMapped.ToList().Add(products);

                    }
                    return Ok(proMapped);
                }
                else
                    return BadRequest("لا يوجد منتج بهذا الاسم");


            }
            else
                return BadRequest("ادخل اسم المنتج الذى تبحث عنه");
        }

        [HttpGet("FilterSearch")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> FilterSearch(string? ProName, int? CategoryId, string? HexCode, decimal? price, string? size)
        {
            if (!string.IsNullOrWhiteSpace(ProName) || CategoryId.HasValue || !string.IsNullOrWhiteSpace(HexCode) || !string.IsNullOrWhiteSpace(size) || price.HasValue)
            {
                var ProList = await _prodduct.FilterSearsh(ProName, CategoryId, HexCode, price, size);
                if (ProList.Any())
                {
                    var proMapped = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(ProList);
                    foreach (var prod in proMapped)
                    {
                        var product = await _prodduct.Get(prod.Id);
                        var products = await ProductDto.GetProducts(prod, product.ProductSales, product.ProductPhotos, product.Reviews, product.ProductColors);
                        proMapped.ToList().Add(products);

                    }
                    return Ok(proMapped);
                }
                else
                    return BadRequest("لا يوجد عنصر بهذه المواصفات");
            }
            else
            {
                return BadRequest("ادخل المواصفات التي تبحث عنها بالمنتج");
            }
        }
    }
}
