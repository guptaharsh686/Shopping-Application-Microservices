using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using static Mango.Web.Utility.StaticDetails;

namespace Mango.Web.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBaseService _baseService;

        public OrderService(IBaseService baseService)
        {
            _baseService = baseService;
        }


        public async Task<ResponseDto?> CreateOrder(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.POST,
                Url = StaticDetails.OrderAPIBase + $"/api/order/CreateOrder",
                Data = cartDto
            });
        }

        public async Task<ResponseDto?> CreateStripeSession(StripeRequestDto stripeRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.POST,
                Url = StaticDetails.OrderAPIBase + $"/api/order/CreateStripeSession",
                Data = stripeRequestDto
            });
        }

        public async Task<ResponseDto?> GetAllOrder(string? userId)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.GET,
                Url = StaticDetails.OrderAPIBase + $"/api/order/GetOrders?userId={userId}",
            });
        }

        public async Task<ResponseDto?> GetOrder(int orderId)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.GET,
                Url = StaticDetails.OrderAPIBase + $"/api/order/GetOrder/{orderId}",
            });
        }

        public async Task<ResponseDto?> UpdateOrderStatus(int orderId, string newStatus)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.POST,
                Url = StaticDetails.OrderAPIBase + $"/api/order/UpdateOrderStatus/{orderId}",
                Data = newStatus
            });
        }

        public async Task<ResponseDto?> ValidateStripeSession(int orderHeaderId)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.POST,
                Url = StaticDetails.OrderAPIBase + $"/api/order/ValidateStripeSession",
                Data = orderHeaderId
            });
        }
    }
}
