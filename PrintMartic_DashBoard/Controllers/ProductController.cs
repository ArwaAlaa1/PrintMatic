using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrintMartic_DashBoard.Helper.ViewModels;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.Repository;

namespace PrintMartic_DashBoard.Controllers
{
  
    public class ProductController : Controller
    {
        private readonly IUnitOfWork<Product> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> userManager;

        public ProductController(IUnitOfWork<Product> unitOfWork, IMapper mapper ,UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            this.userManager = userManager;
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

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductVM productVM)
        {
            var user = User.Identity.Name;
            var userid =userManager.Users.FirstAsync(n => n.UserName==user); 
           
            var itemMapped = _mapper.Map<ProductVM, Product> (productVM);
            
           _unitOfWork.generic.Add(itemMapped);
            _unitOfWork.Complet();

            return RedirectToAction("Index");
        }
    }
}
