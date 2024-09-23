using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Identity.Client;
using PrintMartic_DashBoard.Helper;
using PrintMartic_DashBoard.Helper.ViewModels;
using PrintMatic.Core;
using PrintMatic.Core.Entities;

namespace PrintMartic_DashBoard.Controllers
{
    [Authorize(AuthenticationSchemes = "Cookies", Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork<Category> _unitOfWork;
		private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public CategoryController(IUnitOfWork<Category> unitOfWork, IMapper mapper
            , IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
			_mapper = mapper;
            _environment = webHostEnvironment;
           
        }
        // Get All Categories --GET
        public async Task<IActionResult> Index()
        {
            try
            {
                var List = await _unitOfWork.generic.GetAllAsync();
                if (List == null) return NotFound();
                return View(List);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        // Get Category By Id --GET
        public async Task<IActionResult> Details(int id)
        {
            var item = await _unitOfWork.generic.GetByIdAsync(id);
            if (item == null) return RedirectToAction(nameof(Index));
            var itemMapped = _mapper.Map<Category, CategoryVM>(item);
            //item.PhotoURL = itemMapped.PhotoURL;
          
            return View(itemMapped);
        }

        //Open the form --Get
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        //Create New Category --post  Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Create(CategoryCrVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    categoryVM.PhotoURL = categoryVM.PhotoFile?.FileName;

                    if (categoryVM.PhotoFile != null)
                    {
                        categoryVM.PhotoURL =  DocumentSetting.UploadFile(categoryVM.PhotoFile, "category");
                    }
                    else
                    {
                        ModelState.AddModelError("PhotoURL", "Please Enter Photo");
                    }
                    var CatMapped = _mapper.Map<CategoryCrVM, Category>(categoryVM);
                    CatMapped.FilePath = Path.Combine(_environment.ContentRootPath,"wwwroot\\Uploads\\category", CatMapped.PhotoURL);

                    _unitOfWork.generic.Add(CatMapped);
                    var count = _unitOfWork.Complet();

                    if (count > 0)
                        TempData["message"] = "تم إضافة تفاصيل القسم بنجاح";
                    else
                        TempData["message"] = "فشلت عملية الإضافه";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }
            }

            return View(categoryVM);
        }


        //Open the form of edit --get
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            return await Details(id);
        }

        //Edit category --post Category/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryVM categoryVM)
        {
            if (ModelState.IsValid) 
            {
                try
                {
                    //var item = await _unitOfWork.generic.GetByIdAsync(id);
                    //if (item.Name == categoryVM.Name && item.PhotoURL == categoryVM.PhotoURL)
                    //{
                    //    return RedirectToAction(nameof(Index));
                    //}
                   // var item = await _unitOfWork.generic.GetByIdAsync(categoryVM.Id);
                    if (categoryVM.PhotoFile != null)
                    {
                        if(System.IO.File.Exists(categoryVM.FilePath))
                        {
                            System.IO.File.Delete(categoryVM.FilePath);
                        }
                        categoryVM.PhotoURL =  DocumentSetting.UploadFile(categoryVM.PhotoFile, "category");
                    }
                   var catMapped = _mapper.Map<CategoryVM, Category>(   categoryVM );
                    catMapped.FilePath = Path.Combine(_environment.ContentRootPath, "wwwroot\\Uploads\\category", catMapped.PhotoURL);
                    _unitOfWork.generic.Update(catMapped);
                    var count = _unitOfWork.Complet();
                    if (count > 0)
                    {
                        TempData["Message"] = $"تم تعديل تفاصيل القسم بنجاح";
                    }
                    return RedirectToAction(nameof(Index));
                }catch(Exception e)
                {
                    ModelState.AddModelError(string.Empty, e.InnerException.Message);
                }
            }
            return View(categoryVM);
        }


        //Open form of delete --Get Category/delete/id
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id);
        }

        //Delete category --post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id , CategoryVM categoryVM)
        {
            try
            {
                var category = _mapper.Map<CategoryVM, Category>(categoryVM);
                if (System.IO.File.Exists(category.FilePath))
                {
                    System.IO.File.Delete(category.FilePath);
                }
                category.IsDeleted = true;
                category.IsActive = false;
                _unitOfWork.generic.Update(category);
                var count = _unitOfWork.Complet();
                if (count > 0)
                {
                   // DocumentSetting.DeleteFile("category", category.PhotoURL);
                    TempData["Message"] = "تم حذف تفاصيل القسم بنجاح";
                }
                return RedirectToAction(nameof(Index));
            }
            catch  
            {
                TempData["Message"] = "فشلت عملية الحذف";
                return View(categoryVM);
            }
        }
    }
}
