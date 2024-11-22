using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrintMatic.Core.Repository.Contract;

namespace PrintMatic.Controllers
{
    
    public class DeliveryController : BaseApiController
    {
        private readonly IOrderRepository _orderRepository;

        public DeliveryController(IOrderRepository orderRepository)
        {
          _orderRepository = orderRepository;
        }

        [HttpGet("ReadyOrders")]
        public async Task<IActionResult> GetReadyOrder()
        {
            var orders=await _orderRepository.GetReadyOrders();
            return Ok(orders);
        }
 
    }
}
