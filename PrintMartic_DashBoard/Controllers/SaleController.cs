using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintMartic_DashBoard.Helper.ViewModels;
using PrintMatic.Core;
using PrintMatic.Core.Entities;

namespace PrintMartic_DashBoard.Controllers
{
    [Authorize(AuthenticationSchemes = "Cookies", Roles = "بائع,Admin")]
    public class SaleController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SaleController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        //Get 
        public async Task<IActionResult> Index()
        {
            var List = await _unitOfWork.Repository<Sale>().GetAllAsync();
            return View(List);
        }

        //Get Details 
        public async Task<IActionResult> Details(int id)
        {
            var item = await _unitOfWork.Repository<Sale>().GetByIdAsync(id);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, SaleVM saleVM)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    if (saleVM.SaleEndDate< saleVM.SaleStartDate)
                    {
                        ModelState.AddModelError("SaleEndDate", "ادخل تاريخ انتهاء آخر");
                    }
                    else
                    {
                        var sale = _mapper.Map<SaleVM, Sale>(saleVM);

                        _unitOfWork.Repository<Sale>().Add(sale);
                        var count = await _unitOfWork.Complet();
                        if (count > 0)
                        {
                            TempData["Message"] = "تم إضافة تفاصيل الخصم بنجاح";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                    
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException?.Message.ToString() ?? ex.Message.ToString());
                }
            }
            TempData["Message"] = "فشلت عملية الإضافه";
            return View(saleVM);
        }

        //Open Form of Edit --get
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _unitOfWork.Repository<Sale>().GetByIdAsync(id);
            if (item == null) return RedirectToAction(nameof(Index));
            var Mapped = _mapper.Map<Sale , SaleVM>(item);
            return View(Mapped);
        }

        //Edit Sale -- post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SaleVM saleVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (saleVM.SaleEndDate < saleVM.SaleStartDate)
                    {
                        ModelState.AddModelError("SaleEndDate", "ادخل تاريخ انتهاء آخر");
                    }
                    else
                    {
                        var sale = _mapper.Map<SaleVM, Sale>(saleVM);

                        _unitOfWork.Repository<Sale>().Update(sale);
                        var count = await _unitOfWork.Complet();
                        if (count > 0)
                        {
                            TempData["Message"] = "تم تعديل تفاصيل الخصم بنجاح";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException?.Message.ToString() ?? ex.Message.ToString());
                }
            }
            TempData["Message"] = "فشلت عملية التعديل";
            return View(saleVM);
        }

        //Open form of delete --get
        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id);
        }

        //Delete Sale --post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, Sale sale) 
        {
            try
            {
                sale.IsDeleted = true;
                sale.IsActive = false;
                _unitOfWork.Repository<Sale>().Update(sale);
                var count =await _unitOfWork.Complet();
                
                if (count > 0)
                {
                    TempData["Message"] = "تم حذف تفاصيل الخصم بنجاح";
                }
                return RedirectToAction(nameof(Index));

            }
            catch 
            {
                TempData["Message"] = "فشلت عملية الحذف";
                return View(sale);
            }
        }
    }
}
