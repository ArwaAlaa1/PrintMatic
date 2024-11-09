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
        private readonly string _imagepath;

        public OrderController(IOrderService orderService
            ,ICartRepository cartRepository
            ,IAddressRepository addressRepository
            ,IMapper mapper
            ,UserManager<AppUser> userManager
            , IWebHostEnvironment webHostEnvironment)
        {
            _orderService = orderService;
            _cartRepository = cartRepository;
            _addressRepository = addressRepository;
            _mapper = mapper;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _imagepath = "https://localhost:7234";
        }

        
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var address=await _addressRepository.GetUserAddress(orderDto.AddressId);
            var addressmapped= _mapper.Map<Core.Entities.Order.Address>(address);
            var user = await _userManager.GetUserAsync(User);
            var order = await _orderService.CreateOrderAsync(user.Email, orderDto.CartId, orderDto.ShippingCostId, addressmapped);
            if (order is null)
                return BadRequest(new { Message= "! فشل تأكيد الطلب" });

            return Ok(new { Message = " ! تمت العمليه بنجاح" });
        }

        [Authorize]
        [HttpGet("UserOrders")]
        public async Task<ActionResult<IEnumerable<OrderReturnDto>>> GetUserOrders()
        {
            var user=await _userManager.GetUserAsync(User);
            var orders = await _orderService.GetOrdersForUserAsync(user.Email);
            
            var mappedOrders = _mapper.Map<List<OrderReturnDto>>(orders);
            return Ok(mappedOrders);

        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<OneOrderReturnDto>> GetOrderForUser(int id)
        {
            //var user = await _userManager.GetUserAsync(User);
            var order = await _orderService.GetOrderForUserAsync(id);
            
            var mappedOrders = _mapper.Map<OneOrderReturnDto>(order);
            return Ok(mappedOrders);

        }

        [Authorize]
        [HttpPut("Cancel/{id}")]
        public async Task<ActionResult<Order>> CancelOrderForUser(int id)
        {
            //var user = await _userManager.GetUserAsync(User);
            var order = await _orderService.CancelOrderForUserAsync(id);
            if (order!=null)
                    return Ok(new
                    {

                        Message = " !تم إلغاء الطلب بنجاح "
                    });
           
            return BadRequest(new
            {

                Message = " !حدث خطأ أثناء إلغاء الطلب "
            });

        }

        [Authorize]
        [HttpPut("ReOrder/{id}")]
        public async Task<ActionResult<Order>> ReOrderForUser(int id)
        {
            //var user = await _userManager.GetUserAsync(User);
            var order = await _orderService.ReOrderForUserAsync(id);
            if (order != null)
                return Ok(new { Message = " !تم إعاده الطلب بنجاح " });

            return BadRequest(new { Message = " !حدث خطأ أثناء إعاده الطلب " });

        }
        [Authorize]
        [HttpGet("OrderItem/{ItemId}")]
        public async Task<ActionResult<OrderItemReturnDto>> GetOrderItem(int ItemId)
        {
            //var user = await _userManager.GetUserAsync(User);
            var orderitem = await _orderService.GetOrderItemForOrder(ItemId);
            if (orderitem != null)
            {
                List<string> urls=new List<string>();
                if (orderitem.ProductItem.Photos != null)
                {
                    
                   List<string> images = ImageService.ConvertJsonToUrls(orderitem.ProductItem.Photos);
                    foreach (var item in images)
                    {
                        urls.Add($"{_imagepath}/Custome/Image/{item}");
                    }
                    
                }
                if (orderitem.ProductItem.FilePdf != null)
                {
                    orderitem.ProductItem.FilePdf = $"{_imagepath}/Custome/Image/{orderitem.ProductItem.FilePdf}";
                }
                    var mappedOrderItem = _mapper.Map<OrderItemReturnDto>(orderitem);
                mappedOrderItem.Photos = urls;
                return Ok(mappedOrderItem);
            }
                

            return BadRequest(new { Message = " !حدث خطأ أثناء استرجاع المنتج  " });

        }
    }
}
