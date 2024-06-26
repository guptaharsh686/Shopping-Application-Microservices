﻿using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using static Mango.Web.Utility.StaticDetails;

namespace Mango.Web.Service
{
    public class CartService : ICartService
    {
        private readonly IBaseService _baseService;

        public CartService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.POST,
                Url = StaticDetails.ShoppingCartAPIBase + $"/api/cart/ApplyCoupon",
                Data = cartDto
            });
        }

        public async Task<ResponseDto?> EmailCart(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.POST,
                Url = StaticDetails.ShoppingCartAPIBase + $"/api/cart/EmailCartRequest",
                Data = cartDto
            });
        }

        public async Task<ResponseDto?> GetCartByUserIDAsync(string userId)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.GET,
                Url = StaticDetails.ShoppingCartAPIBase + "/api/cart/GetCart/" + userId
            }); ;
        }

        public async Task<ResponseDto?> RemoveFromCartAsync(int cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.POST,
                Url = StaticDetails.ShoppingCartAPIBase + $"/api/cart/RemoveCart",
                Data = cartDetailsId
            });
        }


        public async Task<ResponseDto?> UpsertCartAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.POST,
                Url = StaticDetails.ShoppingCartAPIBase + $"/api/cart/CartUpsert",
                Data = cartDto
            });
        }
    }
}
