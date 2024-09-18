using Microsoft.AspNetCore.Mvc;
using PrintMatic.Core;
using PrintMatic.Core.Entities;

namespace PrintMartic_DashBoard.Controllers
{
    public class ProductSizeController : Controller
    {
        private readonly IUnitOfWork<ProductSize> _unitOfSize;

        public ProductSizeController(IUnitOfWork<ProductSize> unitOfSize)
        {
            _unitOfSize = unitOfSize;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DeleteSize(int id)
        {
            try
            {

                var item = await _unitOfSize.size.GetSize_Pro(id);
                
                _unitOfSize.size.Delete(id);
                var count = await _unitOfSize.CompletAsync();
                if (count > 0)
                {
                    ViewData["Message"] = "تم حذف تفاصيل المنتج بنجاح";

                }
                return RedirectToAction("Edit", "Product", new { id = item.ProductId });
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
            }
            return RedirectToAction("Index", "Product");
        }
    }
}