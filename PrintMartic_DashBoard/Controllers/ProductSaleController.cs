using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            IUnitOfWork<Sale> SaleUnit)
        {
            _productSale = productSale;
            _mapper = mapper;
            _proUnit = ProUnit;
            _saleUnit = SaleUnit;
        }
        public async Task<IActionResult> Index()
        {
            var List = await _productSale.GetAllAsync();
            return View(List);
        }

        public async Task<IActionResult> Details(int ProductId , int SaleId)
        {
            var item = await _productSale.GetByIDAsync(ProductId,SaleId);
            if(item == null)
            {
                ViewData["Message"] = "Not Found";
                return RedirectToAction("Index");
            }
           var PSMapped = _mapper.Map<ProductSale , ProductSaleVM>(item);
            return View(PSMapped);
        }


        public async Task<IActionResult> Create()
        {
            var ProList = await _proUnit.generic.GetAllAsync();
            var SaleList = await _saleUnit.generic.GetAllAsync();

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
                    var product = await _proUnit.generic.GetByIdAsync(productSaleVM.ProductId);
                    var sale = await _saleUnit.generic.GetByIdAsync(productSaleVM.SaleId);
                    var paS = _productSale.GetPrice(sale.SaleDiscountPercentage, product.NormalPrice);
                    productSaleVM.PriceAfterSale = paS;
                    var ProSale = _mapper.Map<ProductSaleVM, ProductSale>(productSaleVM);
                    _productSale.Add(ProSale);
                    var count = _productSale.Complet();
                    if (count > 0)
                    {
                        ViewData["Message"] = "ProductSale Created Successfully";
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception ex) 
                {
                    ModelState.AddModelError(string.Empty , ex.InnerException.Message);
                }

            }

            var ProList = await _proUnit.generic.GetAllAsync();
            var SaleList = await _saleUnit.generic.GetAllAsync();
            productSaleVM.Sales = SaleList;
            productSaleVM.Products = ProList;
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
                    var product = await _proUnit.generic.GetByIdAsync(productSaleVM.ProductId);
                    var sale = await _saleUnit.generic.GetByIdAsync(productSaleVM.SaleId);
                    var paS = _productSale.GetPrice(sale.SaleDiscountPercentage, product.NormalPrice); productSaleVM.PriceAfterSale = paS;
                    var ProSale = _mapper.Map<ProductSaleVM, ProductSale>(productSaleVM);
                    _productSale.Update(ProSale);
                    var count = _productSale.Complet();
                    if (count > 0)
                    {
                        ViewData["Message"] = "ProductSale Updated Successfully";
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }

            }

            var ProList = await _proUnit.generic.GetAllAsync();
            var SaleList = await _saleUnit.generic.GetAllAsync();
            productSaleVM.Sales = SaleList;
            productSaleVM.Products = ProList;
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
                    TempData["Message"] = "ProductSale Deleted Successfully";
                }
                return RedirectToAction(nameof(Index));

            }catch
            {
                TempData["Message"] = "Deletion operation failed";
                return View(productSaleVM);
            }
        
        
        }

    }
}
