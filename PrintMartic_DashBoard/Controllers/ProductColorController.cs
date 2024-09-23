using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using PrintMatic.Core;
using PrintMatic.Core.Entities;

namespace PrintMartic_DashBoard.Controllers
{
    public class ProductColorController : Controller
    {
        private readonly IUnitOfWork<ProductColor> _unitofColor;

        public ProductColorController(IUnitOfWork<ProductColor> unitofColor)
        {
            _unitofColor = unitofColor;
        }
        public IActionResult Index()
        {
            return View();
        }

       

        public async Task<IActionResult> DeleteColor(int id)
        {
            try
            {

                var item = await _unitofColor.color.GetColor_Pro(id);
                _unitofColor.color.Delete(id);
                var count =await _unitofColor.CompletAsync();
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
