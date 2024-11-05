using Microsoft.AspNetCore.Mvc;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Repository.Contract;

namespace PrintMartic_DashBoard.Controllers
{
    public class ProductSizeController : Controller
    {
        private readonly IProductSize _size;
        private readonly IUnitOfWork _unitOfWork;

        public ProductSizeController(IProductSize size,IUnitOfWork unitOfWork)
        {
            _size = size;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DeleteSize(int id)
        {
            try
            {

                var item = await _size.GetSize_Pro(id);
                
                _size.Delete(id);
                var count = await _unitOfWork.Complet();
                if (count > 0)
                {
                    ViewData["Message"] = "تم حذف تفاصيل المنتج بنجاح";

                }
                return RedirectToAction("Edit", "Product", new { id = item.ProductId });
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.InnerException?.Message.ToString() ?? ex.Message.ToString();
            }
            return RedirectToAction("Index", "Product");
        }
    }
}