using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
    [Authorize(AuthenticationSchemes = "Cookies")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork<Product> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<Category> _catUnitOfwork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork<ProductColor> _unitOfColor;
        private readonly IUnitOfWork<ProductSize> _unitOfSize;

        public ProductController(IUnitOfWork<Product> unitOfWork, IMapper mapper,
            IUnitOfWork<Category> catUnitOfwork, UserManager<AppUser> userManager,
            IUnitOfWork<ProductColor> unitOfColor,
            IUnitOfWork<ProductSize> unitOfSize)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _catUnitOfwork = catUnitOfwork;
            _userManager = userManager;
            _unitOfColor = unitOfColor;
            _unitOfSize = unitOfSize;
        }



        //Get All Products    Get
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var List = await _unitOfWork.prodduct.GetAllProducts();

            return View(List);
        }
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "Admin")]

        public async Task<IActionResult> WaitingProducts()
        {
            var List = await _unitOfWork.prodduct.GetWaitingProducts();
            return View(List);
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
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "Admin")]

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
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "Admin")]

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
            var ListofProColor = await _unitOfColor.color.GetIdOfProAsync(item.Id);
            if (ListofProColor.Count > 0)
            {
                itemMapped.Colors = ListofProColor;
            }
            var ListofProSize = await _unitOfColor.size.GetIdOfProAsync(item.Id);
            if (ListofProSize.Count > 0)
            {
                itemMapped.Sizes = ListofProSize;
            }
            return View(itemMapped);
        }
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
<<<<<<< HEAD

            //var cookievalue = Request.Cookies["Id"];
            // var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // Get the current user's username
            //   var userName = User.Identity.Name;
=======
>>>>>>> ce0301606de742a5cf94105f56ef58c8b53397f8
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

<<<<<<< HEAD
        [Authorize(AuthenticationSchemes = "Cookies", Roles = ("بائع"))]
=======
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
                    if (productVM.Color == true)
                    {
                        if (productVM.ColorJson != null)
                        {
                            var colors = JsonConvert.DeserializeObject<List<string>>(productVM.ColorJson);
                            if (colors.Count > 0)
                            {
                                foreach (var hexCode in colors)
                                {

                                    ProductColor productColor = new ProductColor()
                                    {
                                        ProductId = itemMapped.Id,
                                        HexCode = hexCode,

                                    };
                                    _unitOfColor.generic.Add(productColor);
                                    _unitOfColor.Complet();

                                }
                            }
                        }
                    }
                    if (productVM.SizeJson != null)
                    {
                        var sizes = JsonConvert.DeserializeObject<List<string>>(productVM.SizeJson);
                        if (sizes?.Count > 0)
                        {
                            foreach (var size in sizes)
                            {
                                ProductSize productSize = new ProductSize()
                                {
                                    ProductId = itemMapped.Id,
                                    Size = size
                                };
                                _unitOfSize.generic.Add(productSize);
                                await _unitOfWork.CompletAsync();
                            }
                        }
                    }
                    if (count > 0)
                    {
                        ViewData["Message"] = "تم إضافة تفاصيل المنتج بنجاح";
                    }
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


        [Authorize(AuthenticationSchemes = "Cookies",Roles =("بائع"))]
>>>>>>> ce0301606de742a5cf94105f56ef58c8b53397f8
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
                    var count = await _unitOfWork.CompletAsync();

                    if (productVM.Color == true)
                    {
                        if (productVM.ColorJson != null)
                        {
                            var colors = JsonConvert.DeserializeObject<List<string>>(productVM.ColorJson);
                            if (colors.Count > 0)
                            {
                                foreach (var hexCode in colors)
                                {

                                    ProductColor productColor = new ProductColor()
                                    {
                                        ProductId = itemMapped.Id,
                                        HexCode = hexCode,

                                    };
                                    _unitOfColor.generic.Add(productColor);
                                    _unitOfColor.Complet();

                                }
                            }
                        }

                    }
                    if (productVM.SizeJson != null)
                    {
                        var sizes = JsonConvert.DeserializeObject<List<string>>(productVM.SizeJson);
                        if (sizes?.Count > 0)
                        {
                            foreach (var size in sizes)
                            {
                                ProductSize productSize = new ProductSize()
                                {
                                    ProductId = itemMapped.Id,
                                    Size = size
                                };
                                _unitOfSize.generic.Add(productSize);
                                await _unitOfWork.CompletAsync();
                            }
                        }
                    }
                    if (count > 0)
                    {
                        ViewData["Message"] = "سيتم التأكيد من بيانات المنتج ثم إضافته";
                    }
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


        
        [Authorize(AuthenticationSchemes = "Cookies", Roles = ("بائع,Admin"))]
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
            var ListofProColor = await _unitOfColor.color.GetIdOfProAsync(item.Id);
            if (ListofProColor.Count > 0)
            {
                itemMapped.Colors = ListofProColor;
            }
            var ListofProSize = await _unitOfColor.size.GetIdOfProAsync(item.Id);
            if (ListofProSize.Count > 0)
            {
                itemMapped.Sizes = ListofProSize;
            }
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
        [Authorize(AuthenticationSchemes = "Cookies", Roles = ("بائع,Admin"))]
        public async Task<IActionResult> Edit(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (User.IsInRole("Admin"))
                    {
                        productVM.Enter = true;
                    }
                    if (productVM.NormalPrice <= 500)
                    {
                        productVM.TotalPrice = productVM.NormalPrice * 1.7m;
                    }
                    else
                    {
                        productVM.TotalPrice = productVM.NormalPrice * 2m;
                    }
                    var ProMapped = _mapper.Map<ProductVM, Product>(productVM);
                    _unitOfWork.generic.Update(ProMapped);
                    var count = _unitOfWork.Complet();
                    if (productVM.Color == true)
                    {
                        if(productVM.ColorJson != null)
                        {
                            var colors = JsonConvert.DeserializeObject<List<string>>(productVM.ColorJson);
                            if (colors.Count > 0)
                            {
                                foreach (var hexCode in colors)
                                {

                                    ProductColor productColor = new ProductColor()
                                    {
                                        ProductId = ProMapped.Id,
                                        HexCode = hexCode,

                                    };
                                    _unitOfColor.generic.Add(productColor);
                                    _unitOfColor.Complet();

                                }
                            }
                        }
                       
                    }
                    if(productVM.SizeJson != null)
                    {
                        var sizes = JsonConvert.DeserializeObject<List<string>>(productVM.SizeJson);
                        if (sizes?.Count > 0)
                        {
                            foreach (var size in sizes)
                            {
                                ProductSize productSize = new ProductSize()
                                {
                                    ProductId = ProMapped.Id,
                                    Size = size
                                };
                                _unitOfSize.generic.Add(productSize);
                                await _unitOfWork.CompletAsync();
                            }
                        }
                    }
                    
                    if (count > 0)
                    {
                        ViewData["Message"] = "تم تعديل تفاصيل المنتج بنجاح";
                    }
                    if (User.IsInRole("Admin"))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    if (User.IsInRole("بائع"))
                    {
                        return RedirectToAction(nameof(YourProducts));
                    }
                  
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
        public async Task<IActionResult> Delete(ProductVM productVM)
        {
            try
            {
                var Product = _mapper.Map<ProductVM, Product>(productVM);
                Product.IsDeleted = true;
                Product.IsActive = false;
                var ListofProColor = await _unitOfColor.color.GetIdOfProAsync(Product.Id);
                foreach (var color in ListofProColor)
                {
                    color.IsDeleted = true;
                    color.IsActive = false;
                    _unitOfColor.generic.Update(color);
                    await _unitOfColor.CompletAsync();
                }
                var ListofProSize = await _unitOfColor.size.GetIdOfProAsync(Product.Id);
                foreach (var size in ListofProSize)
                {
                    size.IsDeleted = true;
                    size.IsActive = false;
                    _unitOfSize.generic.Update(size);
                    await _unitOfSize.CompletAsync();
                }
                _unitOfWork.generic.Update(Product);
                var count =await _unitOfWork.CompletAsync();
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