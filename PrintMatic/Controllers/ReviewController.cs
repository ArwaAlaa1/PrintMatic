using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.DTOS;
using System.ComponentModel.DataAnnotations;

namespace PrintMatic.Controllers
{
   
    public class ReviewController : BaseApiController
    {
        private readonly IUnitOfWork<Review> _unitOfWork;
        private readonly IUnitOfWork<Product> _product;
        private readonly UserManager<AppUser> _userManager;

        public ReviewController(IUnitOfWork<Review> unitOfWork,IUnitOfWork<Product> product, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _product = product;
            _userManager = userManager;
        }

        [HttpPost("AddProductReview")]
        [Authorize]
        public async Task<IActionResult> AddReview(int ProductId ,[Range(1,5)] float rating)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return BadRequest("قم بتسجيل الدخول من فضلك");
                }
                var product = await _product.generic.GetByIdAsync(ProductId);
                if (product == null)
                {
                    return BadRequest("لا يوجد منتج بهذا الكود");
                }
                Review review = new Review()
                {
                    ProductId = ProductId,
                    UserId = user.Id,
                    Rating = rating
                };
                _unitOfWork.generic.Add(review);
                var count = _unitOfWork.Complet();
                if (count > 0)
                {
                    return Ok(new Response()
                    {
                        Message = $"تم اضافة التقييم بنجاح"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new Response()
                {
                    Message = $"فشلت عملية الإضافه {ex.Message.ToString() ?? ex.InnerException?.Message.ToString()}"
                });
            }

            return BadRequest("ادخل كود المنتج والتقييم");
        }
    }
}
