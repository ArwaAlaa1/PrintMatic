using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintMartic_DashBoard.Helper.ViewModels;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Repository.Contract;

namespace PrintMartic_DashBoard.Controllers
{
    
    public class ReviewController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IReviewRepository _review;

        public ReviewController(IUnitOfWork unitOfWork,IMapper mapper,IReviewRepository review )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _review = review;
        }
        public async Task<IActionResult> Index()
        {
            var List =await _review.GetAllIncludeProductAsync();

            return View(List);
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _review.GetIdIncludeProductAsync(id);
            if (item == null) return NotFound();
            var RevMapped = _mapper.Map<Review,ReviewVM>(item);
            return View(RevMapped);
        }

        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id , ReviewVM reviewVM)
        {
            try
            {
                var RevMapped = _mapper.Map<ReviewVM,Review>(reviewVM);
                RevMapped.IsDeleted = true;
                RevMapped.IsActive = false;
                _unitOfWork.Repository<Review>().Update(RevMapped);
                var count = await _unitOfWork.Complet();

                if (count> 0)
            {
                TempData["Message"] = "تم إضافة حذف التعليق بنجاح";
            }
            return RedirectToAction(nameof(Index));

            }
            catch 
            {
                TempData["Message"] = "فشلت عملية الحذف ";
                return View(reviewVM);
            }
        }
    }
}
