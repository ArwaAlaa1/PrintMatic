using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintMartic_DashBoard.Helper;
using PrintMartic_DashBoard.Helper.ViewModels;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Repository.Contract;

namespace PrintMartic_DashBoard.Controllers
{
    [Authorize(AuthenticationSchemes = "Cookies", Roles = "بائع,Admin")]
    public class ProductSaleController : Controller
    {
        private readonly IProductSale _productSale;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProdduct _prodduct;
        private readonly ISaleRepository _sale;

        public ProductSaleController(IProductSale productSale ,IMapper mapper,
            IUnitOfWork unitOfWork , IProdduct prodduct , ISaleRepository sale)
        {
            _productSale = productSale;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _prodduct = prodduct;
            _sale = sale;
        }
        public async Task<IActionResult> Index()
        {
            var List = await _productSale.GetActiveSales();
            return View(List);
        }

        public async Task<IActionResult> Details(int ProductId , int SaleId)
        {
            var item = await _productSale.GetByIDAsync(ProductId,SaleId);
            if(item == null)
            {
                ViewData["Message"] = "لم يتم العثور على هذا العنصر";
                return RedirectToAction("Index");
            }
           var PSMapped = _mapper.Map<ProductSale , ProductSaleVM>(item);
            return View(PSMapped);
        }


        public async Task<IActionResult> Create()
        {
            var list = await _prodduct.GetAllProductswithouttables();
            var SaleList = await _sale.GetActiveSales();
         
            var PSMapped = new ProductSaleVM();
            PSMapped.Products = list;
            PSMapped.Sales = SaleList;
            return View(PSMapped);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductSaleVM productSaleVM)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    if(productSaleVM.ProductId == 0 || productSaleVM.SaleId == 0)
                    {
                        ModelState.AddModelError("PriceAfterSale", "اختر اسم المنتج ونسبة الخصم");
                    }else
                    {
                        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productSaleVM.ProductId);
                        var sale = await _unitOfWork.Repository<Sale>().GetByIdAsync(productSaleVM.SaleId);

                        var paS = _productSale.GetPrice(sale.SaleDiscountPercentage, product.TotalPrice);
                        productSaleVM.PriceAfterSale = paS;
                        var ProSale = _mapper.Map<ProductSaleVM, ProductSale>(productSaleVM);
                        _productSale.Add(ProSale);
                        var count = await _unitOfWork.Complet();
                        if (count > 0)
                        {
                            ViewData["Message"] = "تم إضافة الخصم على المنتج بنجاح";
                        }
                        return RedirectToAction(nameof(Index));
                    }

                }
                catch(Exception ex) 
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException?.Message.ToString() ?? ex.Message.ToString());
               
                }

            }
            var list = await _prodduct.GetAllProductswithouttables();
            var SaleList = await _sale.GetActiveSales();
            productSaleVM.Sales = SaleList;
            productSaleVM.Products = list;
            TempData["Message"] = "فشلت عملية الإضافه";
            return View(productSaleVM);

        }


        public async Task<IActionResult> Edit(int ProductId , int SaleId)
        {
            var item = await _productSale.GetByIDAsync(ProductId, SaleId);
            if (item == null) {
                TempData["Message"] = "Not Found";
                return RedirectToAction(nameof(Index));
            }
            var itemMapped = _mapper.Map<ProductSale , ProductSaleVM>(item);
            var list = await _prodduct.GetAllProductswithouttables();
            var SaleList = await _sale.GetActiveSales();
            itemMapped.Products = list;
            itemMapped.Sales = SaleList;
            return View(itemMapped);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductSaleVM productSaleVM)
        {
            if (ModelState.IsValid) 
            {
                try
                {
                    if (productSaleVM.ProductId == 0 || productSaleVM.SaleId == 0)
                    {
                        ModelState.AddModelError("PriceAfterSale", "اختر اسم المنتج ونسبة الخصم");
                    }
                    else
                    {
                        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productSaleVM.ProductId);
                        var sale = await _unitOfWork.Repository<Sale>().GetByIdAsync(productSaleVM.SaleId);
                        var paS = _productSale.GetPrice(sale.SaleDiscountPercentage, product.TotalPrice); productSaleVM.PriceAfterSale = paS;
                        var ProSale = _mapper.Map<ProductSaleVM, ProductSale>(productSaleVM);
                        _productSale.Update(ProSale);
                        var count = await _unitOfWork.Complet();
                        if (count > 0)
                        {
                            ViewData["Message"] = "تم تعديل الخصم على المنتج بنجاح";
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException?.Message.ToString() ?? ex.Message.ToString());
                }

            }
            var list = await _prodduct.GetAllProductswithouttables();
            var SaleList = await _sale.GetActiveSales();
            productSaleVM.Products = list;
            productSaleVM.Sales = SaleList;
            TempData["Message"] = "فشلت عملية التعديل";
            return View(productSaleVM);

        }


        public async Task<IActionResult> Delete(int ProductId, int SaleId)
        {
            return await Details(ProductId, SaleId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ProductSaleVM productSaleVM) 
        {
            try
            {
                var ProSale = _mapper.Map<ProductSaleVM, ProductSale>(productSaleVM);
               
                _productSale.Delete(ProSale);
                var count = await _unitOfWork.Complet();
                if (count > 0)
                {
                    TempData["Message"] = "تم حذف الخصم على المنتج بنجاح";
                }
                return RedirectToAction(nameof(Index));

            }catch
            {
                TempData["Message"] = "فشلت عملية الحذف";
                return View(productSaleVM);
            }
        }

    }
}
