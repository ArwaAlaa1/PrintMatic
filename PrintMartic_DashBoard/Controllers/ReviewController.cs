using Microsoft.AspNetCore.Mvc;
using PrintMatic.Core;
using PrintMatic.Core.Entities;

namespace PrintMartic_DashBoard.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IUnitOfWork<Review> _unitOfWork;

        public ReviewController(IUnitOfWork<Review> unitOfWork )
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var List =await _unitOfWork.generic.GetAllAsync();

            return View(List);
        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _unitOfWork.generic.GetByIdAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete(int id , Review review)
        {
            try
            { 
             _unitOfWork.generic.Delete(review);
            var count = _unitOfWork.Complet();
            if (count > 0)
            {
                TempData["Message"] = "Review Deleted Successfully";
            }
            return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                TempData["Message"] = "Deletion operation failed";
                return View(review);
            }
        }
    }
}
