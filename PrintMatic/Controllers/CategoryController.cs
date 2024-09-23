using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.DTOS;
using PrintMatic.Repository;

namespace PrintMatic.Controllers
{

    public class CategoryController : BaseApiController
    {
        private readonly IUnitOfWork<Category> _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryController(IUnitOfWork<Category> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
            var CategoryMapped = _mapper.Map<Category , CategoryWithProDetails>(category);
            return Ok(CategoryMapped);
        }

    }
}
