using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Client;
using PrintMartic_DashBoard.Helper;
using PrintMartic_DashBoard.Helper.ViewModels;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.Core.Repository.Contract;
using System;

namespace PrintMartic_DashBoard.Controllers
{
    [Authorize(AuthenticationSchemes = "Cookies")]
    public class ProductPhotoController : Controller
    {
        private readonly IProductPhoto _productPhoto;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<Product> _unitOfWork;
        private IWebHostEnvironment _environment;
     

        public ProductPhotoController(IProductPhoto productPhoto, IMapper mapper
            ,IUnitOfWork<Product> unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _productPhoto = productPhoto;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _environment = webHostEnvironment;
          
        }
        #region Admin
        //Admin : All ProductPhotos
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var List = await _productPhoto.GetAllAsync();
            return View(List);
        }
        //Admin : Waiting ProductPhotos
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "Admin")]
        public async Task<IActionResult> WaitingProductPhotos()
        {
            var List = await _productPhoto.GetWaitingProducts();
            return View(List);
        }
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "Admin")]
        public async Task<IActionResult> Confirm(int ProductId , string Photo)
        {
            try
            {
                var item = await _productPhoto.GetByIDAsync(ProductId , Photo);
                item.Enter = true;
                _productPhoto.Update(item);
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
            return RedirectToAction(nameof(WaitingProductPhotos));

        }
        //Create for Admin 
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var ProList = await _unitOfWork.generic.GetAllAsync();
            ProductPhotosVM product = new ProductPhotosVM();
            product.Products = ProList;
            return View(product);
        }
        //Admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "Admin")]
        public async Task<IActionResult> Create(ProductPhotosVM product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    product.Photo = product.PhotoFile.FileName;
                    if (product.PhotoFile != null)
                    {
                        product.Photo = DocumentSetting.UploadFile(product.PhotoFile, "products");
                    }
                    var ProMa = _mapper.Map<ProductPhotosVM, ProductPhotos>(product);
                    ProMa.FilePath = Path.Combine(_environment.ContentRootPath, "wwwroot\\Uploads\\products", ProMa.Photo);
                    ProMa.Enter = true;
                    _productPhoto.Add(ProMa);
                    var count = _productPhoto.Complet();
                    if (count > 0)
                    {
                        TempData["Message"] = "تم إضافة صورة المنتج بنجاح";
                    }
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }

            }
            var ProList = await _unitOfWork.generic.GetAllAsync();
            product.Products = ProList;
            return View(product);
        }
        #endregion


        #region Company
        //Company
        //----------------------------------
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "بائع")]
        public async Task<IActionResult> YourProductPhotos()
        {
            try
            {
                var user = User.Identity.Name;
                var product = await _productPhoto.GetYourProductPhotos(user);
                return View(nameof(Index), product);
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
                return View("Error");
            }
        }

        [Authorize(AuthenticationSchemes = "Cookies", Roles = "بائع")]
        public async Task<IActionResult> CreateForCompany()
        {
            var UserName = User.Identity?.Name;
            var ProList = await _unitOfWork.prodduct.GetYourProducts(UserName);
            ProductPhotosVM product = new ProductPhotosVM();
            product.Products = ProList;
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(AuthenticationSchemes = "Cookies", Roles = "بائع")]
        public async Task<IActionResult> SaveCreate(ProductPhotosVM product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    product.Photo = product.PhotoFile.FileName;
                    if (product.PhotoFile != null)
                    {
                        product.Photo = DocumentSetting.UploadFile(product.PhotoFile, "products");
                    }
                    var ProMa = _mapper.Map<ProductPhotosVM, ProductPhotos>(product);
                    ProMa.FilePath = Path.Combine(_environment.ContentRootPath, "wwwroot\\Uploads\\products", ProMa.Photo);
                    ProMa.Enter = false;
                    _productPhoto.Add(ProMa);
                    var count = _productPhoto.Complet();
                    if (count > 0)
                    {
                        ViewData["Message"] = "سيتم التأكيد من صورة المنتج ثم إضافته";

                    }
                    return RedirectToAction(nameof(YourProductPhotos));

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }

            }
            var ProList = await _unitOfWork.generic.GetAllAsync();
            product.Products = ProList;
            return View(product);
        }
        #endregion
        public async Task<IActionResult> Details(int ProductId, string Photo)
        {
            var item = await _productPhoto.GetByIDAsync(ProductId, Photo);
            if (item == null)
            {
                TempData["Message"] = "لم يتم العثور على هذا العنصر";
                return RedirectToAction("Index");

            }
            var Mapped = _mapper.Map<ProductPhotos, ProductPhotosVM>(item);

            return View(Mapped);
        }

        [Authorize(AuthenticationSchemes = "Cookies", Roles = "بائع,Admin")]

        public async Task<IActionResult> Edit(int ProductId, string Photo)
        {
            var item = await _productPhoto.GetByIDAsync(ProductId, Photo);
            if (item == null)
            {
                TempData["Message"] = "لم يتم العثور على هذا العنصر";
                return RedirectToAction("Index");

            }
            var Mapped = _mapper.Map<ProductPhotos, ProductPhotosVM>(item);

            if (User.IsInRole("Admin"))
            {
                var ProList = await _unitOfWork.generic.GetAllAsync();
                Mapped.Products = ProList;
            }
            else if(User.IsInRole("بائع"))
            {
                var username = User.Identity.Name;
                var ProList = await _unitOfWork.prodduct.GetYourProducts(username);
                Mapped.Products = ProList;

            }
            return View(Mapped);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductPhotosVM photosVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (photosVM.PhotoFile != null)
                    {
                        if (System.IO.File.Exists(photosVM.FilePath))
                        {
                            System.IO.File.Delete(photosVM.FilePath);
                        }
                            photosVM.Photo = DocumentSetting.UploadFile(photosVM.PhotoFile, "products", _environment.WebRootPath);
                    }
                    var ProMapped = _mapper.Map<ProductPhotosVM, ProductPhotos>(photosVM);
                    ProMapped.FilePath = Path.Combine(_environment.ContentRootPath, "wwwroot\\Uploads\\products", ProMapped.Photo);

                    _productPhoto.Update(ProMapped);
                    var count = _unitOfWork.Complet();
                    if (count > 0)
                    {
                        TempData["Message"] = $"تم تعديل صورة المنتج بنجاح";
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }
            }
            var ProList = await _unitOfWork.generic.GetAllAsync();
            photosVM.Products = ProList;
            return View(photosVM);
        }

        public async Task<IActionResult> Delete(int ProductId, string Photo)
        {
            return await Details(ProductId, Photo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public IActionResult Delete(int ProductId, string Photo, ProductPhotosVM photosVM)
        {
            try
            {
                var photoMap = _mapper.Map<ProductPhotosVM, ProductPhotos>(photosVM);

                _productPhoto.Delete(photoMap);
                var count = _unitOfWork.Complet();
                if (count > 0)
                {
                    // DocumentSetting.DeleteFile("category", category.PhotoURL);
                    TempData["Message"] = "تم حذف صورة المنتج بنجاح";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Message"] = "فشلت عملية الحذف";
                return View(photosVM);
            }

        }
    }
}
