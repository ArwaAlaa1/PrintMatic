using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Repository.Contract;
using PrintMatic.DTOS;

namespace PrintMatic.Controllers
{
    public class CartController : BaseApiController
    {
        private readonly ICartRepository cartRepository;
        private readonly IMapper _mapper;

        public CartController(ICartRepository cartRepository,IMapper mapper)
        {
            this.cartRepository = cartRepository;
           _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerCart>> GetCart(string id)
        {
            var cart=await cartRepository.GetCartAsync(id);
            return Ok( cart ?? new CustomerCart(id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerCart>> UpdateCart(CustomerCartDto cart)
        {
            var mappedcart= _mapper.Map<CustomerCart>(cart);
            var createOrupdateCart=await cartRepository.UpdateCartAsync(mappedcart);
            if (createOrupdateCart == null) return BadRequest();
            return Ok( createOrupdateCart );
        }

        [HttpDelete]
        public async Task/*<ActionResult>*/ DeleteCart(string id)
        {
             await cartRepository.DeleteCartAsync(id);
            /*var isdelete= */
            //if (isdelete)
            //    return Ok();

            //return BadRequest();
        }
    }
}
