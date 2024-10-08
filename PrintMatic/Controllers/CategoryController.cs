using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Repository.Contract;
using PrintMatic.DTOS;
using PrintMatic.Repository;

namespace PrintMatic.Controllers
{

    public class CategoryController : BaseApiController
    {
        private readonly IUnitOfWork<Category> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProductSale _productSale;
        private readonly IUnitOfWork<Review> _unitOfReview;
        private readonly IProductPhoto _productPhoto;
        private readonly IUnitOfWork<ProductColor> _unitOfcolor;

        public CategoryController(IUnitOfWork<Category> unitOfWork, IMapper mapper
            ,IProductSale productSale , IUnitOfWork<Review> unitOfReview , IProductPhoto productPhoto,
            IUnitOfWork<ProductColor> unitOfcolor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productSale = productSale;
            _unitOfReview = unitOfReview;
            _productPhoto = productPhoto;
            _unitOfcolor = unitOfcolor;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAll()
        {
            var list = await _unitOfWork.generic.GetAllAsync();
            var MappedList = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(list);
            return Ok(MappedList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryWithProDetails>> GetById(int id)
        {

            var category = await _unitOfWork.category.GetCategoryWithProduct(id);
            if (category == null)
            {
                return BadRequest("Category doesnot exist");
            }

            var CategoryMapped = _mapper.Map<Category, CategoryWithProDetails>(category);
            foreach (var prod in CategoryMapped.Products)
            {
                var SalesList = await _productSale.GetProByIDAsync(prod.Id);
                var PList = await _productPhoto.GetPhotosOfProduct(prod.Id);
                var Reviews = await _unitOfReview.review.GetReviewsOfPro(prod.Id);
                var Colors = await _unitOfcolor.color.GetIdOfProAsync(prod.Id);
                var products = await ProductDto.GetProducts(prod, SalesList, PList, Reviews,Colors);
                CategoryMapped.Products.ToList().Add(products);
            }
            return Ok(CategoryMapped);
        }


    }
}
