using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
            var Item = await _unitOfWork.generic.GetByIdAsync(id);
            if (Item == null) return NotFound();
            return View(Item);
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
                        TempData["message"] = "Category Added Successfully";
                    else
                        TempData["message"] = "Category Failed";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }
            }

            return View(categoryVM);
        }
    }
}
