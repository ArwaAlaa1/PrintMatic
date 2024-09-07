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
using System.Data;
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
            var List = await _unitOfWork.prodduct.GetAllProducts();

            return View(List);
        }

        public async Task<IActionResult> WaitingProducts()
        {
            var List = await _unitOfWork.prodduct.GetWaitingProducts();
            return View("Index", List);
        }
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "بائع,Admin")]
        public async Task<IActionResult> YourProducts()
        {
            try
            {

                var user = User.Identity.Name;
                var product = await _unitOfWork.prodduct.GetYourProducts(user);
                return View(nameof(Index), product);
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
                return View("Error");
            }
        }

        public async Task<IActionResult> InActiveProducts()
        {

            try
            {
                var product = await _unitOfWork.prodduct.GetInActiveProducts();
                return View(nameof(Index), product);
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
                return View("Error");
            }
        }
        public async Task<IActionResult> Confirm(int id)
        {
            try
            {
                var item = await _unitOfWork.generic.GetByIdAsync(id);
                item.Enter = true;
                _unitOfWork.generic.Update(item);
                var count = _unitOfWork.Complet();
                if (count > 0)
                {
                    ViewData["Message"] = "تم التأكيد";
                }
                else
                    ViewData["Message"] = "فشلت العمليه";
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
            }
            return RedirectToAction(nameof(WaitingProducts));

        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _unitOfWork.prodduct.GetIDProducts(id);
            if (item == null)
            {
                TempData["Message"] = "لم يتم العثور على هذا العنصر";
                return RedirectToAction(nameof(Index));
            }
            var itemMapped = _mapper.Map<Product, ProductVM>(item);
            return View(itemMapped);
        }
        [Authorize(AuthenticationSchemes = "Cookies")]
        public async Task<IActionResult> Create()
        {

            //var cookievalue = Request.Cookies["Id"];
            // var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // Get the current user's username
            //   var userName = User.Identity.Name;
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

        [Authorize(AuthenticationSchemes = "Cookies", Roles = ("بائع"))]
        public async Task<IActionResult> CreateForCompany()
        {

            //   var userName = User.Identity.Name;
            ProductVM ProductVM = new ProductVM();
            var List = await _catUnitOfwork.generic.GetAllAsync();
            ProductVM.Categories = List;
            var Username = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(Username);
            ProductVM.UserId = user.Id;
            return View("CreateForCompany", ProductVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(AuthenticationSchemes = "Cookies", Roles = ("بائع"))]
        public async Task<IActionResult> SaveCreate(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (productVM.NormalPrice <= 500)
                    {
                        productVM.TotalPrice = productVM.NormalPrice * 1.7m;
                    }
                    else
                    {
                        productVM.TotalPrice = productVM.NormalPrice * 2m;
                    }
                    productVM.Enter = false;
                    var itemMapped = _mapper.Map<ProductVM, Product>(productVM);


                    _unitOfWork.generic.Add(itemMapped);
                    var count = _unitOfWork.Complet();

                    ViewData["Message"] = "سيتم التأكيد من بيانات المنتج ثم إضافته";

                    return RedirectToAction(nameof(YourProducts));

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(AuthenticationSchemes = "Cookies", Roles = ("Admin"))]
        public async Task<IActionResult> Create(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (productVM.NormalPrice <= 500)
                    {
                        productVM.TotalPrice = productVM.NormalPrice * 1.7m;
                    }
                    else
                    {
                        productVM.TotalPrice = productVM.NormalPrice * 2m;
                    }
                    productVM.Enter = true;
                    var itemMapped = _mapper.Map<ProductVM, Product>(productVM);


                    _unitOfWork.generic.Add(itemMapped);
                    var count = _unitOfWork.Complet();

                    ViewData["Message"] = "تم إضافة تفاصيل المنتج بنجاح";

                    return RedirectToAction(nameof(Index));

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
                ViewData["Message"] = "لم يتم العثور على هذا العنصر";
                return RedirectToAction(nameof(Index));
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
                        ViewData["Message"] = "تم تعديل تفاصيل المنتج بنجاح";
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
                if (count > 0)
                {
                    ViewData["Message"] = "تم حذف تفاصيل المنتج بنجاح";
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
