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
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _category;
        private readonly IMapper _mapper;
        private readonly IProdduct _prodduct;

        public CategoryController(IUnitOfWork unitOfWork,ICategoryRepository category, IMapper mapper
            ,IProdduct prodduct
            )
        {
            _unitOfWork = unitOfWork;
            _category = category;
            _mapper = mapper;
            _prodduct = prodduct;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAll()
        {
            var list = await _unitOfWork.Repository<Category>().GetAllAsync();
            var MappedList = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(list);
            return Ok(MappedList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryWithProDetails>> GetById(int id)
        {

            var category = await _category.GetCategoryWithProduct(id);
            if (category == null)
            {
                return BadRequest("Category doesnot exist");
            }

            var CategoryMapped = _mapper.Map<Category, CategoryWithProDetails>(category);
            foreach (var prod in CategoryMapped.Products)
            {
                var product = await _prodduct.Get(prod.Id);
                var products = await ProductDto.GetProducts(prod, product.ProductSales, product.ProductPhotos, product.Reviews, product.ProductColors);
                CategoryMapped.Products.ToList().Add(products);
            }
            return Ok(CategoryMapped);
        }



    }
}
