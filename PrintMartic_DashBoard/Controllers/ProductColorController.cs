using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Repository.Contract;

namespace PrintMartic_DashBoard.Controllers
{
    public class ProductColorController : Controller
    {
        private readonly IProductColor _color;
        private readonly IUnitOfWork _unitOfWork;

        public ProductColorController(IProductColor color , IUnitOfWork unitOfWork)
        {
            _color = color;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

       

        public async Task<IActionResult> DeleteColor(int id)
        {
            try
            {

                var item = await _color.GetColor_Pro(id);
                _color.Delete(id);
                var count =await _unitOfWork.Complet();
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
