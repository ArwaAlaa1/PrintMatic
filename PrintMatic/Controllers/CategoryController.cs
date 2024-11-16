using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.Core.Repository.Contract;
using PrintMatic.DTOS;
using PrintMatic.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PrintMatic.Controllers
{

    public class CategoryController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _category;
        private readonly IMapper _mapper;
        private readonly IProdduct _prodduct;
        private readonly IFavouriteRepository _favourite;
        private readonly UserManager<AppUser> _user;

        public CategoryController(IUnitOfWork unitOfWork, ICategoryRepository category, IMapper mapper
            , IProdduct prodduct, IFavouriteRepository favourite, UserManager<AppUser> user
            )
        {
            _unitOfWork = unitOfWork;
            _category = category;
            _mapper = mapper;
            _prodduct = prodduct;
            _favourite = favourite;
            _user = user;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAll()
        {
            var list = await _unitOfWork.Repository<Category>().GetAllAsync();
            var MappedList = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDTO>>(list);
            return Ok(MappedList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryWithProDetails>> GetById(int id, string? token)
        {
            try
            {
                // Fetch the category with its products
                var category = await _category.GetCategoryWithProduct(id);
                if (category == null)
                {
                    return BadRequest("Category does not exist.");
                }
                
                var categoryMapped = _mapper.Map<Category, CategoryWithProDetails>(category);

      
                var updatedProducts = new List<ProductDto>();
                foreach (var prod in categoryMapped.Products)
                {
                    var product = await _prodduct.Get(prod.Id);
                    if (product != null)
                    {
                        var productDto = await ProductDto.GetProducts(prod, product.ProductSales, product.ProductPhotos, product.Reviews, product.ProductColors);

                        updatedProducts.Add(productDto);
                    }
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

                // Replace the original products list with the updated one
                categoryMapped.Products = updatedProducts;

                return Ok(categoryMapped);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message ?? ex.InnerException?.Message);
            }
        }

    }
}