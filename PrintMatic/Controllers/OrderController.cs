using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrintMatic.Core;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.Core.Entities.Order;
using PrintMatic.Core.Repository.Contract;
using PrintMatic.Core.Services;
using PrintMatic.DTOS.OrderDTOS;

namespace PrintMatic.Controllers
{
   
    public class OrderController :BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly ICartRepository _cartRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(IOrderService orderService
            ,ICartRepository cartRepository
            ,IAddressRepository addressRepository
            ,IMapper mapper
            ,UserManager<AppUser> userManager)
        {
            _orderService = orderService;
            _cartRepository = cartRepository;
            _addressRepository = addressRepository;
            _mapper = mapper;
            _userManager = userManager;
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
                return BadRequest("فشل فى الطلب");

            return Ok(order);
        }
    }
}
