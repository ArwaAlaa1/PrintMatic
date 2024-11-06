using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrintMatic.Core;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Order;
using PrintMatic.Core.Repository.Contract;
using PrintMatic.DTOS.OrderDTOS;
using PrintMatic.Repository;

namespace PrintMatic.Controllers
{
    public class ShippingCostController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartRepository _cartRepository;

        public ShippingCostController(IMapper mapper,IUnitOfWork unitOfWork,ICartRepository cartRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _cartRepository = cartRepository;
        }

        [Authorize]
        [HttpGet("ShippingCost")]
        public async Task<ActionResult<IEnumerable<ShippingCost>>> GetAllShippingCosts()
        {
            var Costs = await _unitOfWork.Repository<ShippingCost>().GetAllAsync();
            var costmapper = _mapper.Map<IEnumerable<ShippingCost>,IEnumerable<ShippingCostDto>>(Costs);
            return Ok(costmapper);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CustomerCart>> ChooseCost(string CartId,int ShippingId)
        {
            var Shipping= await _unitOfWork.Repository<ShippingCost>().GetByIdAsync(ShippingId);
            var cart=await _cartRepository.GetCartAsync(CartId);
            cart.orderSammery.ShippingPrice=Shipping.Cost;
            var updatedcart=await _cartRepository.UpdateCartAsync(cart);
            return Ok(updatedcart);
        }


    }
}
