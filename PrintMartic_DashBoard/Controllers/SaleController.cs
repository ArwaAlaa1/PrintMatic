using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrintMartic_DashBoard.Helper.ViewModels;
using PrintMatic.Core;
using PrintMatic.Core.Entities;

namespace PrintMartic_DashBoard.Controllers
{
    public class SaleController : Controller
    {
        private readonly IUnitOfWork<Sale> _unitOfWork;
        private readonly IMapper _mapper;

        public SaleController(IUnitOfWork<Sale> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Get 
        public async Task<IActionResult> Index()
        {
            var List = await _unitOfWork.generic.GetAllAsync();
            return View(List);
        }

        //Get Details 
        public async Task<IActionResult> Details(int id)
        {
            var item = await _unitOfWork.generic.GetByIdAsync(id);
            if (item == null) return RedirectToAction("Index");
            return View(item);
        }

        //Open Form of Create
        public IActionResult Create()
        {
            return View();
        }

        //Create Sale --Post
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(int id, SaleVM saleVM)
        {
            
            if (ModelState.IsValid)
            {
                try
                {

                    if (DateTime.Now >saleVM.SaleStartDate && DateTime.Now < saleVM.SaleEndDate)
                    {
                        saleVM.IsOnSale = true;
                    }
                    else
                        saleVM.IsOnSale = false;
                    var sale = _mapper.Map<SaleVM, Sale>(saleVM);
                    _unitOfWork.generic.Add(sale);
                    var count = _unitOfWork.Complet();
                    if (count > 0)
                    {
                        TempData["Message"] = "Sale Created Successfully";
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }
            }
            return View(saleVM);
        }

        //Open Form of Edit --get
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _unitOfWork.generic.GetByIdAsync(id);
            if (item == null) return RedirectToAction(nameof(Index));
            var Mapped = _mapper.Map<Sale , SaleVM>(item);
            return View(Mapped);
        }

        //Edit Sale -- post
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(int id, SaleVM saleVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (DateTime.Now > saleVM.SaleStartDate && DateTime.Now < saleVM.SaleEndDate)
                    {
                        saleVM.IsOnSale = true;
                    }
                    else
                        saleVM.IsOnSale = false;
                    var sale = _mapper.Map<SaleVM, Sale>(saleVM);

                    _unitOfWork.generic.Update(sale);
                    var count = _unitOfWork.Complet();
                    if (count > 0)
                    {
                        TempData["Message"] = "Sale Updated Successfully";
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty , ex.InnerException.Message);
                }
            }
            TempData["Message"] = "The modification operation failed";
            return View(saleVM);
        }

        //Open form of delete --get
        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id);
        }

        //Delete Sale --post
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete(int id, Sale sale) 
        {
            try
            {
                _unitOfWork.generic.Delete(sale);
                var count =_unitOfWork.Complet();
                if (count > 0)
                {
                    TempData["Message"] = "Sale Deleted Successfully";
                }
                return RedirectToAction(nameof(Index));

            }
            catch 
            {
                TempData["Message"] = "Deletion operation failed";
                return View(sale);
            }
        }
    }
}
