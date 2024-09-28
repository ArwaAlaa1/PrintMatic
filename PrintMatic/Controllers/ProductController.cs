using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.Core.Repository.Contract;
using PrintMatic.DTOS;

namespace PrintMatic.Controllers
{

    public class ProductController : BaseApiController
    {
        private readonly IUnitOfWork<Product> _unitOfWork;
        private readonly IUnitOfWork<ProductColor> _unitofcolor;
        private readonly IUnitOfWork<ProductSize> _unitofSize;
        private readonly IMapper _mapper;
        private readonly IProductPhoto _productPhoto;
        private readonly IProductSale _productSale;
        private readonly IUnitOfWork<Review> _unitOfReview;
        private readonly UserManager<AppUser> _user;

        public ProductController(IUnitOfWork<Product> unitOfWork, 
            IUnitOfWork<ProductColor> unitofcolor, IUnitOfWork<ProductSize> unitofSize,
            IMapper mapper, IProductPhoto productPhoto, IProductSale productSale, IUnitOfWork<Review> unitOfReview,UserManager<AppUser> user)
        {
            _unitOfWork = unitOfWork;
            _unitofcolor = unitofcolor;
            _unitofSize = unitofSize;
            _mapper = mapper;
            _productPhoto = productPhoto;
            _productSale = productSale;
            _unitOfReview = unitOfReview;
            _user = user;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDetailsDTO>> GetById(int id)
        {
            var product = await _unitOfWork.prodduct.GetIDProducts(id);
            if (product == null) 
            {
                return BadRequest("There is no product with this Id");
            }
            var ProMapped = _mapper.Map<Product, ProductDetailsDTO>(product);
            ProMapped.User = new UserSimpleDetails()
            {
                Id = product.AppUser.Id,
                FilePath = product.AppUser.FilePath,
                UserName = product.AppUser.UserName
            };
            var ProductColors = await _unitofcolor.color.GetIdOfProAsync(ProMapped.Id);
            foreach (var color in ProductColors)
            {
                ProMapped.Colors.Add(color.HexCode);
            }
            var ProSizes = await _unitofSize.size.GetIdOfProAsync(ProMapped.Id);
            foreach (var size in ProSizes)
            {
                ProMapped.Sizes.Add(size.Size);
            }
            var photosList = await _productPhoto.GetPhotosOfProduct(ProMapped.Id);
            foreach (var photo in photosList)
            {
                ProMapped.Photos.Add(photo.FilePath);
            }
            var SalesList = await _productSale.GetProByIDAsync(ProMapped.Id);
            if (SalesList.ToList().Count > 0)
            {
                var list = new List<ProductSale>();
                foreach (var sale in SalesList)
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
            return Ok(ProMapped);

        }

        [HttpGet("GetUserWithHisProducts/{id}")]
        public async Task<ActionResult<UserWithPro>> GetUserWithHisProducts(string id)
        {
            var user = await _user.FindByIdAsync(id);
            if (user == null)
            {
                return BadRequest("There is no User with this Id");
            }
            var ProList = await _unitOfWork.prodduct.GetUserWithHisProducts(id);
            var proMapped = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(ProList);
           var products = await GetSaleAndReview(proMapped);
            return Ok(new UserWithPro()
            {
                Id = user.Id,
                UserName = user.UserName,
                FilePath = user.FilePath,
                ProductsCount = ProList.ToList().Count,
                products = products,
            });
        }

        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetSaleAndReview(IEnumerable<ProductDto> products)
        {
            foreach (var prod in products)
            {
                var SalesList = await _productSale.GetProByIDAsync(prod.Id);
                if (SalesList.ToList().Count > 0)
                {
                    var list = new List<ProductSale>();
                    foreach (var sale in SalesList)
                    {
                        if (sale.Sale.SaleEndDate > DateTime.UtcNow)
                        {
                            list.Add(sale);
                        }
                    }
                    var item = list.FirstOrDefault();
                    if (item != null)
                    {
                        prod.PriceAfterSale = item.PriceAfterSale;
                    }
                }
                var PList = await _productPhoto.GetPhotosOfProduct(prod.Id);
                if (PList.ToList().Count > 0)
                {
                    var pitem = PList.FirstOrDefault();
                    if (pitem != null)
                    {
                        prod.FilePath = pitem.FilePath;
                    }
                }
                var Reviews = await _unitOfReview.review.GetReviewsOfPro(prod.Id);
                float? Rating = 0f;
                foreach (var review in Reviews)
                {
                    if (review != null)
                    {
                        Rating += review.Rating;
                    }

                }
                prod.AvgRating = Rating / 5f;
            }

            return products;
        }

    }
}
