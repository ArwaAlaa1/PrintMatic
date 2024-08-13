using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrintMartic_DashBoard.Helper.ViewModels;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Repository.Contract;

namespace PrintMartic_DashBoard.Controllers
{
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
        public async Task<IActionResult> Index()
        {
            var List = await _productPhoto.GetAllAsync();
            return View(List);
        }

        public async Task<IActionResult> Details(int ProductId, string Photo)
        {
            var item = await _productPhoto.GetByIDAsync(ProductId, Photo);
            if (item == null)
            {
                TempData["Message"] = "Not Found";
                return RedirectToAction("Index");

            }
            var Mapped = _mapper.Map<ProductPhotos, ProductPhotosVM>(item);

            return View(Mapped);
        }

        public async Task<IActionResult> Create()
        {
            var ProList = await _unitOfWork.generic.GetAllAsync();
            ProductPhotosVM product = new ProductPhotosVM();
            product.Products = ProList;
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductPhotosVM product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var ProMa = _mapper.Map<ProductPhotosVM, ProductPhotos>(product);
                    ProMa.Photo = $"images/product/{ProMa.Photo}";
                    //product.PathPhoto = Path.Combine(_environment.ContentRootPath, ProMa.Photo);
                    //_productPhoto.Add(ProMa);
                    var count = _productPhoto.Complet();
                    if (count > 0)
                    {
                        TempData["Message"] = "Photo added successfully";
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

        public async Task<IActionResult> Edit(int ProductId, string Photo)
        {
            var item = await _productPhoto.GetByIDAsync(ProductId, Photo);
            if (item == null)
            {
                TempData["Message"] = "Not Found";
                return RedirectToAction("Index");

            }
            var ProList = await _unitOfWork.generic.GetAllAsync();
            var Mapped = _mapper.Map<ProductPhotos, ProductPhotosVM>(item);
            Mapped.Products = ProList;
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
                    var ProMapped = _mapper.Map<ProductPhotosVM, ProductPhotos>(photosVM);
                    ProMapped.Photo = $"images/product/{ProMapped.Photo}";
                    _productPhoto.Update(ProMapped);
                    var count = _unitOfWork.Complet();
                    if (count > 0)
                    {
                        TempData["Message"] = $"Photo Updated Successfully";
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
                    TempData["Message"] = "Photo Deleted Successfully";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["Message"] = "Deletion operation failed";
                return View(photosVM);
            }

        }
    }
}
