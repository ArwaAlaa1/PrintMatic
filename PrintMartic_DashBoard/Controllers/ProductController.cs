using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrintMartic_DashBoard.Helper.ViewModels;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.DTOS;
using PrintMatic.Repository;
using System.Security.Claims;
using System.Text.Json;

namespace PrintMartic_DashBoard.Controllers
{

    public class ProductController : Controller
    {
        private readonly IUnitOfWork<Product> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<Category> _catUnitOfwork;
        private readonly UserManager<AppUser> _userManager;

        public ProductController(IUnitOfWork<Product> unitOfWork, IMapper mapper,
            IUnitOfWork<Category> catUnitOfwork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _catUnitOfwork = catUnitOfwork;
            _userManager = userManager;
        }



        //Get All Products    Get
        public async Task<IActionResult> Index()
        {
            var List = await _unitOfWork.generic.GetAllAsync();

            return View(List);
        }

        public async Task<IActionResult> WaitingProducts()
        {
            var List = await _unitOfWork.prodduct.GetWaitingProducts();
             return View(List);
        }
        public async Task<IActionResult> Confirm(int id)
        {
            try
            {
                var item = await _unitOfWork.generic.GetByIdAsync(id);
                item.Enter = true;
                _unitOfWork.generic.Update(item);
              var count =  _unitOfWork.Complet();
                if (count > 0) 
                {
                    ViewData["Message"] = "Done";
                }
                else
                    ViewData["Message"] = "Failed";
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
            }
            return RedirectToAction(nameof(WaitingProducts));

        }

        public async Task<IActionResult> Details(int id)
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

            //var cookievalue = Request.Cookies["Id"];
            // var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // Get the current user's username
            //  var userName = User.Identity.Name;
            ProductVM ProductVM = new ProductVM();
            var List = await _catUnitOfwork.generic.GetAllAsync();
            ProductVM.Categories = List;
            List<AppUser> users = new List<AppUser>();
            foreach (var item in _userManager.Users)
            {
                if (item.IsCompany == true)
                {
                    users.Add(item);
                }
            }
            ProductVM.Users = users;

            return View(ProductVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductVM productVM)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    var itemMapped = _mapper.Map<ProductVM, Product>(productVM);


                    _unitOfWork.generic.Add(itemMapped);
                    var count = _unitOfWork.Complet();

                    ViewData["Message"] = "Product Created Successfully";

                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    ViewData["Message"] = ex.InnerException.Message;
                }
                // var userid =userManager.Users.FirstAsync(n => n.UserName==user); 


            }
            var List = await _catUnitOfwork.generic.GetAllAsync();
            productVM.Categories = List;
            List<AppUser> users = new List<AppUser>();
            foreach (var item in _userManager.Users)
            {
                if (item.IsCompany == true)
                {
                    users.Add(item);
                }
            }
            productVM.Users = users;
            return View(productVM);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var item = await _unitOfWork.generic.GetByIdAsync(id);
            if (item == null)
            {
                ViewData["Message"] = "Not Found";
                return RedirectToAction("Index");
            }
            var itemMapped = _mapper.Map<Product, ProductVM>(item);
            var List = await _catUnitOfwork.generic.GetAllAsync();
            itemMapped.Categories = List;
            List<AppUser> users = new List<AppUser>();
            foreach (var user in _userManager.Users)
            {
                if (user.IsCompany == true)
                {
                    users.Add(user);
                }
            }
            itemMapped.Users = users;
            return View(itemMapped);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var ProMapped = _mapper.Map<ProductVM, Product>(productVM);
                    _unitOfWork.generic.Update(ProMapped);
                    var count = _unitOfWork.Complet();
                    if (count > 0)
                    {
                        ViewData["Message"] = "Product Updated Successfully";
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);

                }
            }
            var List = await _catUnitOfwork.generic.GetAllAsync();
            productVM.Categories = List;
            List<AppUser> users = new List<AppUser>();
            foreach (var item in _userManager.Users)
            {
                if (item.IsCompany == true)
                {
                    users.Add(item);
                }
            }
            productVM.Users = users;
            return View(productVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete(ProductVM productVM)
        {
            try
            {
                var Product = _mapper.Map<ProductVM, Product>(productVM);
                Product.IsDeleted = true;
                _unitOfWork.generic.Update(Product);
                var count = _unitOfWork.Complet();
                if(count > 0)
                {
                    ViewData["Message"] = "Deleted Successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.InnerException.Message;
            }
            return View(productVM);
        }
    }
}
