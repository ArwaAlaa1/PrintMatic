using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.Core.Entities.Order;
using PrintMatic.Core.Repository.Contract;
using PrintMatic.DTOS;
using PrintMatic.DTOS.OrderDTOS;
using PrintMatic.Helper;
using PrintMatic.Repository.Repository;
using StackExchange.Redis;
using System.Drawing;
using System.Linq.Expressions;
using System.Net.WebSockets;

namespace PrintMatic.Controllers
{
    public class CartController : BaseApiController
    {
        private readonly ILogger<CartController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IProdduct _prodduct;
        private readonly IProductSale _productSale;
        private readonly ICartRepository cartRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly CustomeUpload _custome;

        public CartController(ILogger<CartController> logger,IConfiguration configuration
            , IProdduct prodduct, IProductSale productSale, ICartRepository cartRepository, IMapper mapper, UserManager<AppUser> userManager, CustomeUpload custome)
        {
            _logger = logger;
            _configuration = configuration;
            _prodduct = prodduct;
            _productSale = productSale;
            this.cartRepository = cartRepository;
            _mapper = mapper;
            _userManager = userManager;
            _custome = custome;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<CustomerCart>> GetCart()
        {
            try
            {
                // Retrieve the current user
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized(new { Message = "غير مسموح لك بالوصول، يرجى تسجيل الدخول" });
                }

                
                var cart = await cartRepository.GetCartAsync(user.Id);
                if (cart == null || cart.Items == null || !cart.Items.Any())
                {
                    return Ok(new { Message = "لا توجد منتجات فالعربة الان" });
                }

                
                foreach (var item in cart.Items)
                {
                    if (item?.Photos != null && item.Photos.Any())
                    {
                        for (int i = 0; i < item.Photos.Count; i++)
                        {
                            item.Photos[i] = $"{_configuration["ApiBaseUrl"]}/Custome/Image/{item.Photos[i]}";
                        }
                    }

                    if (!string.IsNullOrEmpty(item?.FilePdf))
                    {
                        item.FilePdf = $"{_configuration["ApiBaseUrl"]}/Custome/Pdfs/{item.FilePdf}";
                    }
                }

                return Ok(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while Retriving cart");
                return BadRequest( new { Message = "حدث خطأ أثناء استرجاع العربة. حاول مرة أخرى لاحقًا." });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CustomerCart>> AddToCart([FromForm] CartItemsDto Item)
        {
            try
            {
                // Retrieve the user ID
                var userId = _userManager.GetUserId(User);
                var cartadd = await cartRepository.GetCartAsync(userId);

                // Retrieve product details
                var product = await _prodduct.GetProductWithPhotos(Item.ProductId);
                if (product == null)
                {
                    return NotFound(new { Message = "المنتج غير موجود" });
                }

                
                decimal priceaftersale = 0.0M;
                var SalesList = await _productSale.GetProByIDAsync(Item.ProductId);
                if (SalesList.ToList().Count > 0)
                {
                    var list = new List<ProductSale>();
                    foreach (var sale in SalesList)
                    {
                        if (sale.Sale.SaleEndDate > DateTime.UtcNow)
                        {
                            list.Add(sale);
                        }
                    }
                    var item = list.FirstOrDefault();
                    if (item != null)
                    {
                        priceaftersale = item.PriceAfterSale;
                    }
                }


                var mappeditem = _mapper.Map<CartItems>(Item);
                mappeditem.ProductName = product.Name;
                mappeditem.ImageUrl = $"{_configuration["DashboardUrl"]}/Uploads/products/{product.ProductPhotos.FirstOrDefault()?.Photo}";
                mappeditem.Price = Item.Type == "عادى" ? product.TotalPrice : product.UrgentPrice;
                mappeditem.PriceAfterSale = priceaftersale;
                mappeditem.MinData = Item.Type == "عادى" ? product.NormalMinDate : product.UrgentMinDate;
                mappeditem.MaxData = Item.Type == "عادى" ? product.NormalMaxDate : product.UrgentMaxDate;

               
                if (cartadd.IsProductUnique(mappeditem) && Item.Photos == null && Item.FilePdf == null)
                {
                    cartadd.Items.Add(mappeditem);
                }
                else if (!cartadd.IsProductUnique(mappeditem) && Item.Photos == null && Item.FilePdf == null)
                {
                    var existingItem = cartadd.FindMatchingProduct(mappeditem);
                    existingItem.Quantity += 1;
                }
                else if (Item.Photos != null || Item.FilePdf != null)
                {
                    // Handle photo and file uploads
                    if (Item.Photos != null)
                    {
                        mappeditem.Photos = await _custome.Upload(Item.Photos);
                    }
                    else if (Item.FilePdf != null)
                    {
                        mappeditem.FilePdf = await _custome.UploadFile(Item.FilePdf);
                    }
                    cartadd.Items.Add(mappeditem);
                }

                
                var createOrupdateCart = await cartRepository.UpdateCartAsync(cartadd);
                if (createOrupdateCart == null)
                {
                    return BadRequest(new { Message = "حدث خطأ فى اضافه المنتج للعربة" });
                }

                return Ok(new { Message = "تم اضافه المنتج للعربة" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while Add item to cart");
                return BadRequest( new { Message = "حدث خطأ أثناء إضافة المنتج للعربة. حاول مرة أخرى لاحقًا." });
            }
        }

        [Authorize]
        [HttpDelete("Item")]
        public async Task<ActionResult> DeleteItem(string cartid, string Itemid)
        {
            try
            {
                // Retrieve the cart by cart ID
                var cart = await cartRepository.GetCartAsync(cartid);
                if (cart == null)
                {
                    return NotFound(new { Message = "العربة غير موجودة" });
                }

                // Find the item to remove by Item ID
                var itemToRemove = cart.Items?.FirstOrDefault(item => item.Id == Itemid);
                if (itemToRemove == null)
                {
                    return NotFound(new { Message = "العنصر غير موجود في العربة" });
                }

                // Handle deletion of photos and PDF if they exist
                if (itemToRemove.Photos != null && itemToRemove.Photos.Any())
                {
                    await _custome.Delete(itemToRemove.Photos);
                }

                if (!string.IsNullOrEmpty(itemToRemove.FilePdf))
                {
                    await _custome.DeleteFile(itemToRemove.FilePdf);
                }

                // Remove the item from the cart
                cart.Items.Remove(itemToRemove);

                // Update the cart or delete if empty
                var updatedBasketData = await cartRepository.UpdateCartAsync(cart);
                if (!cart.Items.Any())
                {
                    await cartRepository.DeleteCartAsync(cartid);
                    return Ok(new { Message = "لا توجد منتجات فالعربة الان" });
                }

                return Ok(new { Message = "تم حذف العنصر من العربة" });
            }
            catch (Exception ex)
            {
                
                 _logger.LogError(ex, "Error occurred while deleting item from cart");

                return BadRequest( new { Message = "حدث خطأ أثناء حذف العنصر من العربة. حاول مرة أخرى لاحقًا." });
            }
        }



        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> DeleteCart(string cartid)
        {
            try
            {
                await cartRepository.DeleteCartAsync(cartid);
                return Ok(new { Message = "لا توجد منتجات فالعربة الان" });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting item from cart");

                return BadRequest(new { Message = "حدث خطأ أثناء حذف العنصر من العربة. حاول مرة أخرى لاحقًا." });
            }

        }


        [Authorize]
        [HttpPost("Increase")]
        public async Task<ActionResult> Increase(string cartid, string ItemId)
        {
            try
            {
                // Retrieve the cart by ID
                var cart = await cartRepository.GetCartAsync(cartid);
                if (cart == null)
                {
                    return NotFound(new { Message = "العربة غير موجودة" });
                }

                // Find the item to increase quantity
                var itemToIncrease = cart.Items.FirstOrDefault(item => item.Id == ItemId);
                if (itemToIncrease == null)
                {
                    return NotFound(new { Message = "العنصر غير موجود في العربة" });
                }

                // Increase the quantity of the item
                itemToIncrease.Quantity += 1;

                // Update the cart
                var updatedBasketData = await cartRepository.UpdateCartAsync(cart);
                if (updatedBasketData == null)
                {
                    return BadRequest(new { Message = "حدث خطأ أثناء تحديث العربة" });
                }

                return Ok(new { Message = "تم زيادة الكمية بنجاح" });
            }
            catch (Exception ex)
            {
                // Log the exception for internal diagnostics
                _logger.LogError(ex, "Error occurred while increasing item quantity in the cart.");

                // Return a generic error message to the user
                return BadRequest( new { Message = "حدث خطأ غير متوقع. يرجى المحاولة لاحقًا." });
            }
        }

        [Authorize]
        [HttpPost("Decrease")]
        public async Task<ActionResult> Decrease(string cartid, string ItemId)
        {
            try
            {
                // Retrieve the cart by ID
                var cart = await cartRepository.GetCartAsync(cartid);
                if (cart == null)
                {
                    return NotFound(new { Message = "العربة غير موجودة" });
                }

                // Find the item to decrease quantity
                var itemToDecrease = cart.Items.FirstOrDefault(item => item.Id == ItemId);
                if (itemToDecrease == null)
                {
                    return NotFound(new { Message = "العنصر غير موجود في العربة" });
                }

                
                if (itemToDecrease.Quantity > 1)
                {
                    itemToDecrease.Quantity -= 1;
                }
                else
                {
                    return BadRequest(new { Message = "لا يمكن تقليل الكمية لأقل من واحد" });
                }

               
                var updatedBasketData = await cartRepository.UpdateCartAsync(cart);
                if (updatedBasketData == null)
                {
                    return BadRequest(new { Message = "حدث خطأ أثناء تحديث العربة" });
                }

                return Ok(new { Message = "تم تقليل الكمية بنجاح" });
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "Error occurred while decreasing item quantity in the cart.");

             
                return BadRequest( new { Message = "حدث خطأ غير متوقع. يرجى المحاولة لاحقًا." });
            }
        }

        //[Authorize]
        //[HttpPost]
        //public async Task<ActionResult<CustomerCart>> AddToCart([FromForm] CartItemsDto Item)
        //{
        //    var userId = _userManager.GetUserId(User);

        //    var cartadd = await cartRepository.GetCartAsync(userId);
        //    var mappeditem = _mapper.Map<CartItems>(Item);

        //    if (cartadd.IsProductUnique(mappeditem) && Item.Photos == null && Item.FilePdf == null)
        //    {
        //        cartadd.Items.Add(mappeditem);

        //    }
        //    else if (!cartadd.IsProductUnique(mappeditem) && Item.Photos == null && Item.FilePdf == null)
        //    {
        //        var item = cartadd.FindMatchingProduct(mappeditem);
        //        item.Quantity += 1;

        //    }
        //    else if (Item.Photos != null || Item.FilePdf != null)
        //    {

        //        if (Item.Photos != null)
        //        {
        //            var photos = await _custome.Upload(Item.Photos);
        //            mappeditem.Photos = photos;

        //        }
        //        else if (Item.FilePdf != null)
        //        {
        //            var filepdf = await _custome.UploadFile(Item.FilePdf);
        //            mappeditem.FilePdf = filepdf;
        //        }
        //        cartadd.Items.Add(mappeditem);
        //    }

        //    var createOrupdateCart = await cartRepository.UpdateCartAsync(cartadd);
        //    if (createOrupdateCart == null) return BadRequest();
        //    return Ok(createOrupdateCart);
        //}


    }
}
