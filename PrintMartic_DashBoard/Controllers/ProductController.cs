using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PrintMartic_DashBoard.Helper;
using PrintMartic_DashBoard.Helper.ViewModels;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.Core.Repository.Contract;
using PrintMatic.Repository;
using System.Data;
using System.Drawing;
using System.Security.Claims;
using System.Text.Json;

namespace PrintMartic_DashBoard.Controllers
{
    [Authorize(AuthenticationSchemes = "Cookies")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProdduct _prodduct;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IProductColor _color;
        private readonly IProductSize _size;
        private readonly IWebHostEnvironment _environment;
        private readonly IProductPhoto _photo;

        public ProductController(IUnitOfWork unitOfWork,IProdduct prodduct, IMapper mapper,
             UserManager<AppUser> userManager,
            IProductColor color,
            IProductSize size , IWebHostEnvironment webHostEnvironment,IProductPhoto photo)
        {
            _unitOfWork = unitOfWork;
            _prodduct = prodduct;
            _mapper = mapper;
            _userManager = userManager;
            _color = color;
            _size = size;
            _environment = webHostEnvironment;
            _photo = photo;
        }



        //Get All Products    Get
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var List = await _prodduct.GetAllProducts();

            return View(List);
        }

        [Authorize(AuthenticationSchemes = "Cookies", Roles = "Admin")]
        public async Task<IActionResult> WaitingProducts()
        {
            var List = await _prodduct.GetWaitingProducts();
            return View(List);
        }

        [Authorize(AuthenticationSchemes = "Cookies", Roles = "بائع,Admin")]
        public async Task<IActionResult> YourProducts()
        {
            try
            {
                var user = User.Identity.Name;
                var product = await _prodduct.GetYourProducts(user);
                return View(nameof(Index), product);
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.InnerException?.Message.ToString() ?? ex.Message.ToString();
                return View("Error");
            }
        }

        [Authorize(AuthenticationSchemes = "Cookies", Roles = "Admin")]
        public async Task<IActionResult> InActiveProducts()
        {

            try
            {
                var product = await _prodduct.GetInActiveProducts();
                return View(nameof(Index), product);
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.InnerException?.Message.ToString() ?? ex.Message.ToString();
                return View("Error");
            }
        }

        [Authorize(AuthenticationSchemes = "Cookies", Roles = "Admin")]
        public async Task<IActionResult> RestoreData(int id)
        {
            var item = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            item.IsDeleted = false;
            item.IsActive = true;
            item.Enter = true;
            var ListofProColor = await _color.GetIdOfProAsync(item.Id);
            foreach (var color in ListofProColor)
            {
                color.IsDeleted = false;
                color.IsActive = true;
                _unitOfWork.Repository<ProductColor>().Update(color);
                await _unitOfWork.Complet();
            }
            var ListofProSize = await _size.GetIdOfProAsync(item.Id);
            foreach (var size in ListofProSize)
            {
                size.IsDeleted = false;
                size.IsActive = true;
                _unitOfWork.Repository<ProductSize>().Update(size);
                await _unitOfWork.Complet();
            }
            _unitOfWork.Repository<Product>().Update(item);
            await _unitOfWork.Complet();
            return RedirectToAction(nameof(InActiveProducts));
        }
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "Admin")]
        public async Task<IActionResult> Confirm(int id)
        {
            try
            {
                var item = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
                item.Enter = true;
                _unitOfWork.Repository<Product>().Update(item);
                var count = await _unitOfWork.Complet();
                var Photos = await _photo.GetPhotosOfProduct(item.Id);
                foreach (var photo in Photos)
                {
                    photo.Enter = true;
                    _photo.Update(photo);
                    await _unitOfWork.Complet();
                }
                if (count > 0)
                {
                    ViewData["Message"] = "تم التأكيد";
                }
                else
                    ViewData["Message"] = "فشلت العمليه";
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.InnerException?.Message.ToString() ?? ex.Message.ToString();
            }
            return RedirectToAction(nameof(WaitingProducts));

        }

        public async Task<IActionResult> Details(int id)
        {
            var item = await _prodduct.GetIDProducts(id);
            if (item == null)
            {
                TempData["Message"] = "لم يتم العثور على هذا العنصر";
                return RedirectToAction(nameof(Index));
            }
            var itemMapped = _mapper.Map<Product, ProductVM>(item);
            if (item.ProductColors.Any())
            {
                itemMapped.Colors = item.ProductColors.ToList();
            }
            if (item.ProductSizes.Any())
            {
                itemMapped.Sizes = item.ProductSizes.ToList();
            }

            if (item.ProductPhotos.Any())
            {
                foreach (var photo in item.ProductPhotos)
                {
                    itemMapped.photos.Add(photo.Photo);
                }
            }
            return View(itemMapped);
        }

        [Authorize(AuthenticationSchemes = "Cookies", Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            try
            {
                ProductVM ProductVM = new ProductVM();
                var List = await _unitOfWork.Repository<Category>().GetAllAsync();
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
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.InnerException?.Message.ToString() ?? ex.Message.ToString());
                ;
                return RedirectToAction(nameof(Index));
            }

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
                    productVM.UrgentPrice = productVM.TotalPrice * 2m;
                   productVM.Enter = true;
                    var itemMapped = _mapper.Map<ProductVM, Product>(productVM);
                   _unitOfWork.Repository<Product>().Add(itemMapped);
                    var count = await _unitOfWork.Complet();
                    if (productVM.PhotoFiles.Any())
                    {
                        foreach (var file in productVM.PhotoFiles)
                        {
                            ProductPhotos productPhotos = new ProductPhotos();
                            productPhotos.Photo = file.FileName;
                            productPhotos.Photo = DocumentSetting.UploadFile(file, "products");
                            productPhotos.ProductId = itemMapped.Id;
                            productPhotos.FilePath = Path.Combine(_environment.ContentRootPath, "wwwroot\\Uploads\\products", productPhotos.Photo);
                            productPhotos.Enter = true;
                            _photo.Add(productPhotos);
                            _photo.Complet();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("PhotoFiles", "ادخل صور هذا المنتج");
                    }
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
                                    _unitOfWork.Repository<ProductColor>().Add(productColor);
                                  await _unitOfWork.Complet();

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
                                _unitOfWork.Repository<ProductSize>().Add(productSize);
                                await _unitOfWork.Complet();
                            }
                        }
                    }
                    if (count > 0)
                    {
                        ViewData["Message"] = "تم إضافة تفاصيل المنتج بنجاح";
                    }
                    return RedirectToAction(nameof(Details),new {id = itemMapped.Id});

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("CategoryId", "ادخل اسم القسم من فضلك");
                    ModelState.AddModelError("UserId", "ادخل اسم البائع من فضلك");
                    ModelState.AddModelError(string.Empty, ex.InnerException?.Message.ToString() ?? ex.Message.ToString());
                }
                // var userid =userManager.Users.FirstAsync(n => n.UserName==user); 

            }
            var List = await _unitOfWork.Repository<Category>().GetAllAsync();
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

        public async Task<IActionResult> CreateForCompany()
        {

            //   var userName = User.Identity.Name;
            ProductVM ProductVM = new ProductVM();
            var List = await _unitOfWork.Repository<Category>().GetAllAsync();
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
                    productVM.UrgentPrice = productVM.TotalPrice * 2m;
                    productVM.Enter = false;
                    var itemMapped = _mapper.Map<ProductVM, Product>(productVM);
                    _unitOfWork.Repository<Product>().Add(itemMapped);
                    var count = await _unitOfWork.Complet();
                    if (productVM.PhotoFiles.Any())
                    {
                        foreach (var file in productVM.PhotoFiles)
                        {
                            ProductPhotos productPhotos = new ProductPhotos();
                            productPhotos.Photo = file.FileName;
                            productPhotos.Photo = DocumentSetting.UploadFile(file, "products");
                            productPhotos.ProductId = itemMapped.Id;
                            productPhotos.FilePath = Path.Combine(_environment.ContentRootPath, "wwwroot\\Uploads\\products", productPhotos.Photo);
                            productPhotos.Enter = false;
                            _photo.Add(productPhotos);
                            await _unitOfWork.Complet();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("PhotoFiles", "ادخل صور هذا المنتج");
                    }


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
                                    _unitOfWork.Repository<ProductColor>().Add(productColor);
                                    await _unitOfWork.Complet();

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
                                _unitOfWork.Repository<ProductSize>().Add(productSize);
                                await _unitOfWork.Complet();
                            }
                        }
                    }
                    if (count > 0)
                    {
                        ViewData["Message"] = "سيتم التأكيد من بيانات المنتج ثم إضافته";
                    }
                    return RedirectToAction(nameof(Details), new { id = itemMapped.Id });

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("CategoryId", "ادخل اسم القسم من فضلك");
                    ModelState.AddModelError(string.Empty, ex.InnerException?.Message.ToString() ?? ex.Message.ToString());
                }
                // var userid =userManager.Users.FirstAsync(n => n.UserName==user); 

            }
            var List = await _unitOfWork.Repository<Category>().GetAllAsync();
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
            var item = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            if (item == null)
            {
                ViewData["Message"] = "لم يتم العثور على هذا العنصر";
                return RedirectToAction(nameof(Index));
            }
            
            var itemMapped = _mapper.Map<Product, ProductVM>(item);
            var List = await _unitOfWork.Repository<Category>().GetAllAsync();
            itemMapped.Categories = List;
            var ListofProColor = await _color.GetIdOfProAsync(item.Id);
            if (ListofProColor.Count > 0)
            {
                itemMapped.Colors = ListofProColor;
            }
            var ListofProSize = await _size.GetIdOfProAsync(item.Id);
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
            var Photos = await _photo.GetPhotosOfProduct(item.Id);
            if (Photos.Any())
            {
                foreach(var url in Photos)
                {
                    itemMapped.photos.Add(url.Photo);
                }
            }
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
                    productVM.UrgentPrice = productVM.TotalPrice * 2m;
                    var ProMapped = _mapper.Map<ProductVM, Product>(productVM);
                    _unitOfWork.Repository<Product>().Update(ProMapped);
                    var count = await _unitOfWork.Complet();

                    if (productVM.PhotoFiles.Any())
                    {
                        foreach (var file in productVM.PhotoFiles)
                        {
                            ProductPhotos photos = new ProductPhotos();
                            photos.Photo = file.FileName;
                            photos.Photo = DocumentSetting.UploadFile(file, "products");
                            photos.ProductId = ProMapped.Id;
                            photos.FilePath = Path.Combine(_environment.ContentRootPath, "wwwroot\\Uploads\\products", photos.Photo);
                            if (User.IsInRole("Admin"))
                            {
                                photos.Enter = true;
                            }else
                                photos.Enter = false;
                           
                            _photo.Add(photos);
                            await _unitOfWork.Complet();
                        }

                    }


                    if (productVM.ColorJson != null)
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
                                    _unitOfWork.Repository<ProductColor>().Add(productColor);
                                    await _unitOfWork.Complet();

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
                                _unitOfWork.Repository<ProductSize>().Add(productSize);
                                await _unitOfWork.Complet();
                            }
                        }
                    }
                    
                    if (count > 0)
                    {
                        ViewData["Message"] = "تم تعديل تفاصيل المنتج بنجاح";
                        return RedirectToAction(nameof(Details), new { id = ProMapped.Id });

                    }



                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("CategoryId", "ادخل اسم القسم من فضلك");
                    ModelState.AddModelError(string.Empty, ex.InnerException?.Message.ToString() ?? ex.Message.ToString());

                }
            }
            var List = await _unitOfWork.Repository<Category>().GetAllAsync();
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
                var ListofProColor = await _color.GetIdOfProAsync(Product.Id);
                foreach (var color in ListofProColor)
                {
                    color.IsDeleted = true;
                    color.IsActive = false;
                    _unitOfWork.Repository<ProductColor>().Update(color);
                    await _unitOfWork.Complet();
                }
                var ListofProSize = await _size.GetIdOfProAsync(Product.Id);
                foreach (var size in ListofProSize)
                {
                    size.IsDeleted = true;
                    size.IsActive = false;
                    _unitOfWork.Repository<ProductSize>().Update(size);
                    await _unitOfWork.Complet();
                }
                _unitOfWork.Repository<Product>().Update(Product);
                var count =await _unitOfWork.Complet();
                var Photos = await _photo.GetPhotosOfProduct(productVM.Id);
                foreach (var photo in Photos)
                {
                    if (System.IO.File.Exists(photo.FilePath))
                    {
                        System.IO.File.Delete(photo.FilePath);
                    }

                    _photo.Delete(photo);
                    await _unitOfWork.Complet();

                }

                if (count > 0)
                {
                    ViewData["Message"] = "تم حذف تفاصيل المنتج بنجاح";
                    return RedirectToAction(nameof(Index));
                }
               
            }
            catch (Exception ex)
            {

                ViewData["Message"] = ex.InnerException?.Message.ToString() ?? ex.Message.ToString();
            }
            return View(productVM);
        }

        [HttpPost]
        public async Task<JsonResult> ToggleStatus(int id, bool isActive)
        {
            try
            {
                // Retrieve the item from the database (e.g., using Entity Framework)
                var item =await _unitOfWork.Repository<Product>().GetByIdAsync(id);
                if (item == null)
                {
                    return Json(new { success = false });
                }

                // Update the IsActive property
                item.IsActive= isActive;
                await _unitOfWork.Complet();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Handle any errors
                return Json(new { success = false, message = ex.Message });
            }
        }


    }

}