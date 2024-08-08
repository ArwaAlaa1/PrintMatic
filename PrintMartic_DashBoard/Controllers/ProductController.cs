using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrintMartic_DashBoard.Helper.ViewModels;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Repository;

namespace PrintMartic_DashBoard.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork<Product> _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork<Product> unitOfWork, IMapper mapper )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //Get All Products    Get
        public async Task<IActionResult> Index()
        {
            var List = await _unitOfWork.generic.GetAllAsync();

            return View(List);
        } 

        public async Task<IActionResult> Details(int id )
        {
            var item = await _unitOfWork.generic.GetByIdAsync(id);
            if (item == null)
            {
                TempData["Message"] = "Not Found";
                return RedirectToAction("Index");
            }
            var itemMapped = _mapper.Map<Product, ProductVM>(item);
            return View(itemMapped);
        }

    }
}
