using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Storage;
using PrintMartic_DashBoard.Helper;
using PrintMartic_DashBoard.Helper.ViewModels;
using PrintMatic.Core;
using PrintMatic.Core.Entities;

namespace PrintMartic_DashBoard.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork<Category> _unitOfWork;
		private readonly IMapper _mapper;

		public CategoryController(IUnitOfWork<Category> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
			_mapper = mapper;
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
        public IActionResult Create(CategoryVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    categoryVM.PhotoURL = categoryVM.PhotoFile.FileName;

                    if (categoryVM.PhotoFile != null)
                    {
                        categoryVM.PhotoURL = DocumentSetting.UploadFile(categoryVM.PhotoFile, "category");
                    }
                    var CatMapped = _mapper.Map<CategoryVM, Category>(categoryVM);
                    CatMapped.PhotoURL = $"images/category/{CatMapped.PhotoURL}";
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
                    if (categoryVM.PhotoFile != null)
                    {
                        //categoryVM.PhotoURL = categoryVM.PhotoFile.FileName;
                        //DocumentSetting.DeleteFile("category", categoryVM.PhotoURL);
                        categoryVM.PhotoURL = DocumentSetting.UploadFile(categoryVM.PhotoFile, "category");
                    }
                    var catMapped = _mapper.Map<CategoryVM, Category>(categoryVM);
                    catMapped.PhotoURL = $"images/category/{catMapped.PhotoURL}";
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
                category.IsDeleted = true;
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
