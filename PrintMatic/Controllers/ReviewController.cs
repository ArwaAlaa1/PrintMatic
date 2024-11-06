using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.Core.Repository.Contract;
using PrintMatic.DTOS;
using System.ComponentModel.DataAnnotations;

namespace PrintMatic.Controllers
{
   
    public class ReviewController : BaseApiController
    {
        private readonly IReviewRepository _review;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProdduct _product;
        private readonly UserManager<AppUser> _userManager;

        public ReviewController(IReviewRepository review,IUnitOfWork unitOfWork,IProdduct product, UserManager<AppUser> userManager)
        {
            _review = review;
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
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(ProductId);
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
                _unitOfWork.Repository<Review>().Add(review);
                var count = await _unitOfWork.Complet();
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
