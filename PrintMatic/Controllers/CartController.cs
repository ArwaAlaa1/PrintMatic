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
using PrintMatic.DTOS.OrderDTOS;
using PrintMatic.Helper;
using PrintMatic.Repository.Repository;
using StackExchange.Redis;
using System.Net.WebSockets;

namespace PrintMatic.Controllers
{
    public class CartController : BaseApiController
    {
       
        private readonly ICartRepository cartRepository;
        private readonly IMapper _mapper;
		private readonly UserManager<AppUser> _userManager;
        private readonly CustomeUpload _custome;

        public CartController(ICartRepository cartRepository,IMapper mapper,UserManager<AppUser> userManager,CustomeUpload custome)
        {
           
            this.cartRepository = cartRepository;
           _mapper = mapper;
			_userManager = userManager;
            _custome = custome;
        }

       
        [HttpGet]
        public async Task<ActionResult<CustomerCart>> GetCart(string? email)
        {
            if (!email.IsNullOrEmpty())
            {
                var user=await _userManager.FindByEmailAsync(email);

                var cart = await cartRepository.GetCartAsync(user.Id);
                if (cart.Items.Count()==0)
                {
					return Ok(new { Message = "لا توجد منتجات فالعربة الان" });
				}
				return Ok(cart);
			}
			
			return Ok( new {Message="لا توجد منتجات فالعربة الان"});
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CustomerCart>> AddToCart([FromForm] CartItemsDto Item)
        {
            var userId =  _userManager.GetUserId(User);
           
            var cartadd = await cartRepository.GetCartAsync(userId) ;
            var mappeditem = _mapper.Map<CartItems>(Item);
           
            if (cartadd.IsProductUnique(mappeditem) && Item.Photos == null && Item.FilePdf == null)
            {
                cartadd.Items.Add(mappeditem);
               
            }else if (!cartadd.IsProductUnique(mappeditem) && Item.Photos == null && Item.FilePdf == null)
            {
                var item=cartadd.FindMatchingProduct(mappeditem);
                item.Quantity += 1;

            }
            else if (Item.Photos != null || Item.FilePdf != null)
            {
                
                if (Item.Photos != null)
                {
                    var photos = await _custome.Upload(Item.Photos);
                    mappeditem.Photos = photos;

                }else if(Item.FilePdf != null)
                {
                    var filepdf = await _custome.UploadFile(Item.FilePdf);
                    mappeditem.FilePdf = filepdf;
                }
                cartadd.Items.Add(mappeditem);
            }
         
            var createOrupdateCart = await cartRepository.UpdateCartAsync(cartadd);
            if (createOrupdateCart == null) return BadRequest();
            return Ok(createOrupdateCart);
        }

        [Authorize]
		[HttpDelete]
		public async Task<ActionResult<CustomerCart>> DeleteItem(string cartid,string Itemid)
		{
			var cart = await cartRepository.GetCartAsync(cartid);
			var itemToRemove = cart.Items.FirstOrDefault(item => item.Id == Itemid);
             
                
            if (itemToRemove != null)
			{
				if (itemToRemove.Photos.Count() > 0)
				{
					await _custome.Delete(itemToRemove.Photos);
                    if (itemToRemove.FilePdf != null)
                    {
                        await _custome.DeleteFile(itemToRemove.FilePdf);
                    }
                }
				cart.Items.Remove(itemToRemove);
			}
			var updatedBasketData = await cartRepository.UpdateCartAsync(cart);
			if (cart.Items.Count() == 0)
			{
				await cartRepository.DeleteCartAsync(cartid);
				return Ok(new { Message = "لا توجد منتجات فالعربة الان" });
			}
            return Ok(updatedBasketData);

		}


        [Authorize]
        [HttpPost("Increase")]
        public async Task<ActionResult<CustomerCart>> Increase(string cartid, string ItemId)
        {
            var cart = await cartRepository.GetCartAsync(cartid);
            var itemToIncrease = cart.Items.FirstOrDefault(item => item.Id == ItemId);


            if (itemToIncrease != null)
            {
                itemToIncrease.Quantity += 1;
                
            }
            var updatedBasketData = await cartRepository.UpdateCartAsync(cart);

            return Ok(updatedBasketData);

        }
        [Authorize]
        [HttpPost("Decrease")]
        public async Task<ActionResult<CustomerCart>> Decrease(string cartid, string ItemId)
        {
            var cart = await cartRepository.GetCartAsync(cartid);
            var itemToDecrease = cart.Items.FirstOrDefault(item => item.Id == ItemId);


            if (itemToDecrease != null && itemToDecrease.Quantity !=1)
            {
                itemToDecrease.Quantity -= 1;

            }
            var updatedBasketData = await cartRepository.UpdateCartAsync(cart);

            return Ok(updatedBasketData);

        }


     }
}
