using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrintMatic.Core;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.Core.Entities.Order;
using PrintMatic.Core.Repository.Contract;
using PrintMatic.Core.Services;
using PrintMatic.DTOS.OrderDTOS;
using PrintMatic.Services;

using static StackExchange.Redis.Role;

namespace PrintMatic.Controllers
{
   
    public class OrderController :BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly ICartRepository _cartRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<OrderController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string? _imagepath;

        public OrderController(IOrderService orderService
            ,ICartRepository cartRepository
            ,IAddressRepository addressRepository
            ,IMapper mapper
            ,UserManager<AppUser> userManager
            , IWebHostEnvironment webHostEnvironment,
            ILogger<OrderController> logger,
                        IConfiguration configuration)
        {
            _orderService = orderService;
            _cartRepository = cartRepository;
            _addressRepository = addressRepository;
            _mapper = mapper;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
            _configuration = configuration;
            _imagepath = _configuration["ApiBaseUrl"];
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateOrder(OrderDto orderDto)
        {
            try
            {
                
                var address = await _addressRepository.GetUserAddress(orderDto.AddressId);
                if (address == null)
                {
                    return NotFound(new { Message = "العنوان غير موجود" });
                }

                
                var addressMapped = _mapper.Map<Core.Entities.Order.Address>(address);

               
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized(new { Message = "المستخدم غير مصرح له" });
                }

                
                var order = await _orderService.CreateOrderAsync(user.Email, orderDto.CartId, orderDto.ShippingCostId, addressMapped);
                if (order == null)
                {
                    return BadRequest(new { Message = "! فشل تأكيد الطلب" });
                }

                return Ok(new { Message = "تمت العملية بنجاح!" });
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "Error occurred while creating the order.");

                return BadRequest( new { Message = "حدث خطأ غير متوقع. يرجى المحاولة لاحقًا." });
            }
        }

        [Authorize]
        [HttpGet("UserOrders")]
        public async Task<ActionResult<IEnumerable<OrderReturnDto>>> GetUserOrders()
        {
            try
            {
                
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Unauthorized(new { Message = "المستخدم غير مصرح له" });
                }

                
                var orders = await _orderService.GetOrdersForUserAsync(user.Email);
                if (orders == null || !orders.Any())
                {
                    return NotFound(new { Message = "لا توجد طلبات للمستخدم" });
                }

                
                var mappedOrders = _mapper.Map<List<OrderReturnDto>>(orders);

                
                return Ok(mappedOrders);
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, "Error occurred while retrieving orders for user.");

                
                return BadRequest( new { Message = "حدث خطأ غير متوقع. يرجى المحاولة لاحقًا." });
            }
        }


        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<OneOrderReturnDto>> GetOrderForUser(int id)
        {
            try
            {
               
                var user = await _userManager.GetUserAsync(User);

               
                var order = await _orderService.GetOrderForUserAsync(id);

                
                if (order == null)
                {
                    _logger.LogWarning($"Order with id {id} not found for user {user?.Email}");
                    return NotFound(new { Message = "الطلب غير موجود" }); 
                }

                
                var mappedOrder = _mapper.Map<OneOrderReturnDto>(order);

                
                return Ok(mappedOrder);
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, $"An error occurred while retrieving order with id {id}");

                
                return BadRequest( new { Message = "حدث خطأ أثناء استرجاع البيانات. حاول مرة أخرى لاحقاً." }); 
            }
        }

        [Authorize]
        [HttpPut("Cancel/{id}")]
        public async Task<ActionResult<Order>> CancelOrderForUser(int id)
        {
            try
            {
               
                var order = await _orderService.CancelOrderForUserAsync(id);

                if (order != null)
                {
                   
                    return Ok(new
                    {
                        Message = " !تم إلغاء الطلب بنجاح "  
                    });
                }

                return BadRequest(new
                {
                    Message = " !حدث خطأ أثناء إلغاء الطلب "  
                });
            }
            catch (Exception ex)
            {
                
                _logger.LogError(ex, $"An error occurred while canceling order with id {id}");

             
                return BadRequest( new
                {
                    Message = "حدث خطأ أثناء إلغاء الطلب. حاول مرة أخرى لاحقاً."  
                });
            }
        }

        [Authorize]
        [HttpPut("ReOrder/{id}")]
        public async Task<ActionResult<Order>> ReOrderForUser(int id)
        {
            try
            {
               
                var order = await _orderService.ReOrderForUserAsync(id);

                if (order != null)
                {
                    
                    return Ok(new { Message = " !تم إعاده الطلب بنجاح " });  
                }

               
                return BadRequest(new { Message = " !حدث خطأ أثناء إعاده الطلب " });  
            }
            catch (Exception ex)
            {
               
                _logger.LogError(ex, $"An error occurred while reordering the order with id {id}");

               
                return BadRequest( new
                {
                    Message = "حدث خطأ أثناء إعاده الطلب. حاول مرة أخرى لاحقاً."  
                });
            }
        }

        [Authorize]
        [HttpDelete("DeleteOrder/{id}")]
        public async Task<ActionResult<Order>> DeleteOrderForUser(int id)
        {
            try
            {
               
                var order = await _orderService.DeleteOrderForUserAsync(id);

     
                if (order != null)
                {
                    return Ok(new { Message = " !تم حذف الطلب بنجاح " }); 
                }

              
                return BadRequest(new { Message = " !حدث خطأ أثناء حذف الطلب " }); 
            }
            catch (Exception ex)
            {
               
                _logger.LogError(ex, $"An error occurred while deleting the order with id {id}");

               
                return BadRequest( new
                {
                    Message = "حدث خطأ أثناء حذف الطلب. حاول مرة أخرى لاحقاً."  
                });
            }
        }

        //[Authorize]
        //[HttpGet("OrderItem/{ItemId}")]
        //public async Task<ActionResult<OrderItemReturnDto>> GetOrderItem(int ItemId)
        //{
        //    //var user = await _userManager.GetUserAsync(User);
        //    var orderitem = await _orderService.GetOrderItemForOrder(ItemId);
        //    if (orderitem != null)
        //    {
        //        List<string> urls=new List<string>();
        //        if (orderitem.ProductItem.Photos != null)
        //        {

        //           List<string> images = ImageService.ConvertJsonToUrls(orderitem.ProductItem.Photos);
        //            foreach (var item in images)
        //            {
        //                urls.Add($"{_imagepath}/Custome/Image/{item}");
        //            }

        //        }
        //        if (orderitem.ProductItem.FilePdf != null)
        //        {
        //            orderitem.ProductItem.FilePdf = $"{_imagepath}/Custome/Image/{orderitem.ProductItem.FilePdf}";
        //        }
        //            var mappedOrderItem = _mapper.Map<OrderItemReturnDto>(orderitem);
        //        mappedOrderItem.Photos = urls;
        //        return Ok(mappedOrderItem);
        //    }


        //    return BadRequest(new { Message = " !حدث خطأ أثناء استرجاع المنتج  " });

        //}
    }
}
