using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrintMatic.Core;
using PrintMatic.Core.Entities.Order;
using PrintMatic.Core.Repository.Contract;
using PrintMatic.DTOS.OrderDTOS;

namespace PrintMatic.Controllers
{
    
    public class DeliveryController : BaseApiController
    {
        private readonly IOrderRepository _orderRepository;
        private readonly Core.IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeliveryController(IOrderRepository orderRepository,IUnitOfWork unitOfWork,IMapper mapper)
        {
          _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("ReadyOrders")]
        public async Task<IActionResult> GetReadyOrder()
        {
            var orders=await  _orderRepository.GetReadyOrders();
            var ordermappers=_mapper.Map<List<DeliveryOrderDto>>(orders);
            return Ok(ordermappers);
        }

        [HttpPut("Shipping")]
        public async Task<IActionResult> ShippingOrder(int orderid)
        {
            var order = await _orderRepository.GetByIdAsync(orderid);
            order.Status = OrderStatus.Shipping;
           await  _unitOfWork.Complet();
            return Ok(order);
        }
        [HttpPut("Delivered")]
        public async Task<IActionResult> DeliveredOrder(int orderid)
        {
            var order = await _orderRepository.GetByIdAsync(orderid);
            order.Status = OrderStatus.Deliverd;
            await _unitOfWork.Complet();
            return Ok(order);
        }

    }
}
