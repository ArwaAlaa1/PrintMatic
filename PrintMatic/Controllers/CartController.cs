using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PrintMatic.Core.Entities;
using PrintMatic.Core.Entities.Identity;
using PrintMatic.Core.Repository.Contract;
using PrintMatic.DTOS;
using StackExchange.Redis;

namespace PrintMatic.Controllers
{
    public class CartController : BaseApiController
    {
        private readonly ICartRepository cartRepository;
        private readonly IMapper _mapper;
		private readonly UserManager<AppUser> _userManager;

		public CartController(ICartRepository cartRepository,IMapper mapper,UserManager<AppUser> userManager)
        {
            this.cartRepository = cartRepository;
           _mapper = mapper;
			_userManager = userManager;
		}

       
        [HttpGet]
        public async Task<ActionResult<CustomerCart>> GetCart(string? email)
        {
            if (!email.IsNullOrEmpty())
            {
                var user=await _userManager.FindByEmailAsync(email);
                var cart = await cartRepository.GetCartAsync(user.Id);
                if (cart==null)
                {
					return Ok(new { Message = "لا توجد منتجات فالعربة الان" });
				}
				return Ok(cart);
			}
			
			return Ok( new {Message="لا توجد منتجات فالعربة الان"});
        }
        [Authorize]
        [HttpPost]
        //[Consumes("multipart/form-data")]
        public async Task<ActionResult<CustomerCart>> UpdateCart([FromForm] CustomerCartDto cart)
        {
            var user = await _userManager.GetUserAsync(User);
            cart.Id = user.Id;
            
            var mappedcart = _mapper.Map<CustomerCart>(cart);
            var createOrupdateCart = await cartRepository.UpdateCartAsync(mappedcart);
            if (createOrupdateCart == null) return BadRequest();
            return Ok(createOrupdateCart);
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
