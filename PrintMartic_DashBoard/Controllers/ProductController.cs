using Microsoft.AspNetCore.Mvc;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Repository;

namespace PrintMartic_DashBoard.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork<Product> _unitOfWork;

        public ProductController(IUnitOfWork<Product> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //Get All Products    Get
        public async Task<IActionResult> GetAll()
        {
            return View();
        } 

    }
}
