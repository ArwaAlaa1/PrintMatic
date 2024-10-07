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
        private readonly IUnitOfWork<Product> _proUnit;
        private readonly IUnitOfWork<Sale> _saleUnit;

        public ProductSaleController(IProductSale productSale ,IMapper mapper, IUnitOfWork<Product> ProUnit ,
            IUnitOfWork<Sale> SaleUnit )
        {
            _productSale = productSale;
            _mapper = mapper;
            _proUnit = ProUnit;
            _saleUnit = SaleUnit;
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
            var list = await _proUnit.generic.GetAllAsync();
           var ProList = list.Where(x => x.Enter == true);
            var SaleList = await _saleUnit.Sale.GetActiveSales();
         
            var PSMapped = new ProductSaleVM();
            PSMapped.Products = ProList;
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
                        var product = await _proUnit.generic.GetByIdAsync(productSaleVM.ProductId);
                        var sale = await _saleUnit.Sale.GetByIdAsync(productSaleVM.SaleId);

                        var paS = _productSale.GetPrice(sale.SaleDiscountPercentage, product.TotalPrice);
                        productSaleVM.PriceAfterSale = paS;
                        var ProSale = _mapper.Map<ProductSaleVM, ProductSale>(productSaleVM);
                        _productSale.Add(ProSale);
                        var count = _productSale.Complet();
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
            var ProList = await _proUnit.generic.GetAllAsync();
            var SaleList = await _saleUnit.generic.GetAllAsync();
            productSaleVM.Sales = SaleList;
            productSaleVM.Products = ProList;
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
            var ProList = await _proUnit.generic.GetAllAsync();
            var SaleList = await _saleUnit.generic.GetAllAsync();
            itemMapped.Products = ProList;
            itemMapped.Sales = SaleList;
            return View(itemMapped);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductSaleVM productSaleVM)
        {
            if (!ModelState.IsValid) 
            {
                try
                {
                    if (productSaleVM.ProductId == 0 || productSaleVM.SaleId == 0)
                    {
                        ModelState.AddModelError("PriceAfterSale", "اختر اسم المنتج ونسبة الخصم");
                    }
                    else
                    {
                        var product = await _proUnit.generic.GetByIdAsync(productSaleVM.ProductId);
                        var sale = await _saleUnit.generic.GetByIdAsync(productSaleVM.SaleId);
                        var paS = _productSale.GetPrice(sale.SaleDiscountPercentage, product.TotalPrice); productSaleVM.PriceAfterSale = paS;
                        var ProSale = _mapper.Map<ProductSaleVM, ProductSale>(productSaleVM);
                        _productSale.Update(ProSale);
                        var count = _productSale.Complet();
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

            var ProList = await _proUnit.generic.GetAllAsync();
            var SaleList = await _saleUnit.generic.GetAllAsync();
            productSaleVM.Sales = SaleList;
            productSaleVM.Products = ProList;
            TempData["Message"] = "فشلت عملية التعديل";
            return View(productSaleVM);

        }


        public async Task<IActionResult> Delete(int ProductId, int SaleId)
        {
            return await Details(ProductId, SaleId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(ProductSaleVM productSaleVM) 
        {
            try
            {
                var ProSale = _mapper.Map<ProductSaleVM, ProductSale>(productSaleVM);
               
                _productSale.Delete(ProSale);
                var count = _productSale.Complet();
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
