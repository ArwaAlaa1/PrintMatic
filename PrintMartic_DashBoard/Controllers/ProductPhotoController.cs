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
        private readonly IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _environment;
     

        public ProductPhotoController(IProductPhoto productPhoto, IMapper mapper
            ,IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _productPhoto = productPhoto;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _environment = webHostEnvironment;
          
        }
        
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
                var count = await _unitOfWork.Complet();
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
            return RedirectToAction(nameof(WaitingProductPhotos));

        }


        ////Create for Admin 
        //[Authorize(AuthenticationSchemes = "Cookies", Roles = "Admin")]
        //public async Task<IActionResult> Create()
        //{
        //    var ProList = await _unitOfWork.generic.GetAllAsync();
        //    ProductPhotosVM product = new ProductPhotosVM();
        //    product.Products = ProList;
        //    return View(product);
        //}
        //Admin
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(AuthenticationSchemes = "Cookies", Roles = "Admin")]
        //public async Task<IActionResult> Create(ProductPhotosVM product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if (product.PhotoFile == null)
        //            {
        //                ModelState.AddModelError("Photo", "ادخل الصوره");
        //            }
        //            else
        //            {
        //                product.Photo = product.PhotoFile.FileName;
        //                product.Photo = DocumentSetting.UploadFile(product.PhotoFile, "products");

        //                var ProMa = _mapper.Map<ProductPhotosVM, ProductPhotos>(product);
        //                ProMa.FilePath = Path.Combine(_environment.ContentRootPath, "wwwroot\\Uploads\\products", ProMa.Photo);
        //                ProMa.Enter = true;
        //                _productPhoto.Add(ProMa);
        //                var count = _productPhoto.Complet();
        //                if (count > 0)
        //                {
        //                    TempData["Message"] = "تم إضافة صورة المنتج بنجاح";
        //                }
        //                return RedirectToAction("Index");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError("ProductId", "ادخل اسم المنتج");
        //            ModelState.AddModelError(string.Empty, ex.InnerException?.Message.ToString() ?? ex.Message.ToString());
        //        }

        //    }
        //    var ProList = await _unitOfWork.generic.GetAllAsync();
        //    product.Products = ProList;
        //    TempData["Message"] = "فشلت عملية الإضافه";
        //    return View(product);
        //}
        //#endregion


        //#region Company
        ////Company
        ////----------------------------------
        //[Authorize(AuthenticationSchemes = "Cookies", Roles = "بائع")]
        //public async Task<IActionResult> YourProductPhotos()
        //{
        //    try
        //    {
        //        var user = User.Identity.Name;
        //        var product = await _productPhoto.GetYourProductPhotos(user);
        //        return View(nameof(Index), product);
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewData["Message"] = ex.InnerException?.Message.ToString() ?? ex.Message.ToString();
        //        return View("Error");
        //    }
        //}

        //[Authorize(AuthenticationSchemes = "Cookies", Roles = "بائع")]
        //public async Task<IActionResult> CreateForCompany()
        //{
        //    var UserName = User.Identity?.Name;
        //    var ProList = await _unitOfWork.prodduct.GetYourProducts(UserName);
        //    ProductPhotosVM product = new ProductPhotosVM();
        //    product.Products = ProList;
        //    return View(product);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(AuthenticationSchemes = "Cookies", Roles = "بائع")]
        //public async Task<IActionResult> SaveCreate(ProductPhotosVM product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if(product.ProductId == 0)
        //            {
        //                ModelState.AddModelError("ProductId", "ادخل اسم المنتج");
        //            }
        //            if (product.PhotoFile == null)
        //            {
        //                ModelState.AddModelError("Photo", "ادخل الصوره");
        //            }
        //            else
        //            {
        //                product.Photo = product.PhotoFile.FileName;
        //                product.Photo = DocumentSetting.UploadFile(product.PhotoFile, "products");

        //                var ProMa = _mapper.Map<ProductPhotosVM, ProductPhotos>(product);
        //                ProMa.FilePath = Path.Combine(_environment.ContentRootPath, "wwwroot\\Uploads\\products", ProMa.Photo);
        //                ProMa.Enter = false;
        //                _productPhoto.Add(ProMa);
        //                var count = _productPhoto.Complet();
        //                if (count > 0)
        //                {
        //                    ViewData["Message"] = "سيتم التأكيد من صورة المنتج ثم إضافته";

        //                }
        //                return RedirectToAction(nameof(YourProductPhotos));
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError(string.Empty, ex.InnerException?.Message.ToString() ?? ex.Message.ToString());
        //        }

        //    }
        //    var ProList = await _unitOfWork.generic.GetAllAsync();
        //    product.Products = ProList;
        //    TempData["Message"] = "فشلت عملية الإضافه";
        //    return View(product);
        //}
        //#endregion
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

        //[Authorize(AuthenticationSchemes = "Cookies", Roles = "بائع,Admin")]

        //public async Task<IActionResult> Edit(int ProductId, string Photo)
        //{
        //    var item = await _productPhoto.GetByIDAsync(ProductId, Photo);
        //    if (item == null)
        //    {
        //        TempData["Message"] = "لم يتم العثور على هذا العنصر";
        //        return RedirectToAction("Index");

        //    }
        //    var Mapped = _mapper.Map<ProductPhotos, ProductPhotosVM>(item);

        //    if (User.IsInRole("Admin"))
        //    {
        //        var ProList = await _unitOfWork.generic.GetAllAsync();
        //        Mapped.Products = ProList;

        //    }
        //    else if(User.IsInRole("بائع"))
        //    {
        //        var username = User.Identity.Name;
        //        var ProList = await _unitOfWork.prodduct.GetYourProducts(username);
        //        Mapped.Products = ProList;

        //    }
        //    return View(Mapped);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[Authorize(AuthenticationSchemes = "Cookies", Roles = "بائع,Admin")]
        //public async Task<IActionResult> Edit(ProductPhotosVM photosVM)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if (photosVM.PhotoFile != null)
        //            {
        //                if (System.IO.File.Exists(photosVM.FilePath))
        //                {
        //                    System.IO.File.Delete(photosVM.FilePath);
        //                }
        //                    photosVM.Photo = DocumentSetting.UploadFile(photosVM.PhotoFile, "products", _environment.WebRootPath);
        //            }
        //            var ProMapped = _mapper.Map<ProductPhotosVM, ProductPhotos>(photosVM);
        //            ProMapped.FilePath = Path.Combine(_environment.ContentRootPath, "wwwroot\\Uploads\\products", ProMapped.Photo);
        //            if (User.IsInRole("Admin"))
        //            {
        //                ProMapped.Enter = true;
        //            }
        //            else
        //            {
        //                ProMapped.Enter = false;
        //            }

        //            _productPhoto.Update(ProMapped);
        //            var count = _unitOfWork.Complet();
        //            if (count > 0 )
        //            {
        //                TempData["Message"] = $"تم تعديل صورة المنتج بنجاح";
        //            }
        //            if (User.IsInRole("Admin"))
        //            {
        //                return RedirectToAction(nameof(Index));
        //            }
        //            else
        //                return RedirectToAction(nameof(YourProductPhotos));
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError(string.Empty, ex.InnerException?.Message.ToString() ?? ex.Message.ToString());
        //        }
        //    }
        //    var ProList = await _unitOfWork.generic.GetAllAsync();
        //    photosVM.Products = ProList;
        //    TempData["Message"] = "فشلت عملية التعديل";
        //    return View(photosVM);
        //}

        public async Task<IActionResult> Delete(int ProductId, string Photo)
        {
            return await Details(ProductId, Photo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int ProductId, string Photo, ProductPhotosVM photosVM)
        {
            try
            {
                if (System.IO.File.Exists(photosVM.FilePath))
                {
                    System.IO.File.Delete(photosVM.FilePath);
                }
                var photoMap = _mapper.Map<ProductPhotosVM, ProductPhotos>(photosVM);

                _productPhoto.Delete(photoMap);
                var count = await _unitOfWork.Complet();
                if (count > 0)
                {
                    // DocumentSetting.DeleteFile("category", category.PhotoURL);
                    TempData["Message"] = "تم حذف صورة المنتج بنجاح";
                }
                return RedirectToAction("Edit", "Product", new { id = ProductId });
            }
            catch
            {
                TempData["Message"] = "فشلت عملية الحذف";
                return View(photosVM);
            }

        }
    }
}
