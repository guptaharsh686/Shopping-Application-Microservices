using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;

        public CartController(ICartService cartService,IOrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
        }


        [Authorize]
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartBasedOnLoggedInUser());
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCartBasedOnLoggedInUser());
        }

        [Authorize]
        [HttpPost("Checkout")]
        public async Task<IActionResult> Checkout(CartDto cartDto)
        {
            //Reload cart
            CartDto cart = await LoadCartBasedOnLoggedInUser();

            cart.CartHeader.Email = cartDto.CartHeader.Email;
            cart.CartHeader.Phone = cartDto.CartHeader.Phone;
            cart.CartHeader.Name = cartDto.CartHeader.Name;

            var response = await _orderService.CreateOrder(cart);
            if(response != null && response.IsSuccess)
            {
                OrderHeaderDto orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));

                //TODO - get stripe session and redirect to stripe to place order
            }
            else
            {
                TempData["error"] = "Please try again";
            }
            return View(LoadCartBasedOnLoggedInUser());

        }

        private async Task<CartDto> LoadCartBasedOnLoggedInUser()
        {
            //Get userId of logged in user from claims which are configured in auth controller signed in user.
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

            var response = await _cartService.GetCartByUserIDAsync(userId);

            if (response != null && response.IsSuccess)
            {
                CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
                return cartDto;
            }
           
            return new CartDto();
        }


        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

            var response = await _cartService.RemoveFromCartAsync(cartDetailsId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Item removed sucessfully";
                return RedirectToAction(nameof(CartIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {

            var response = await _cartService.ApplyCouponAsync(cartDto);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon applied sucessfully";
                return RedirectToAction(nameof(CartIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View();

        }


        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            cartDto.CartHeader.CouponCode = "";
            var response = await _cartService.ApplyCouponAsync(cartDto);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon removed sucessfully";
                return RedirectToAction(nameof(CartIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDto cartDto)
        {
            CartDto cart = await LoadCartBasedOnLoggedInUser();
            cart.CartHeader.Email = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value;

            var response = await _cartService.EmailCart(cart);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Email will be processed and sent shortly";
                return RedirectToAction(nameof(CartIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View();

        }

    }
}
