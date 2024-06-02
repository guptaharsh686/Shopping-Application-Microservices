using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using static Mango.Web.Utility.StaticDetails;

namespace Mango.Web.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;

        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> CreateProduct(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.POST,
                Url = StaticDetails.ProductAPIBase + $"/api/product",
                Data = productDto
            });
        }

        public async Task<ResponseDto?> DeleteProduct(int id)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.DELETE,
                Url = StaticDetails.ProductAPIBase + $"/api/product/{id}",
            });
        }

        public async Task<ResponseDto?> GetAllProducts()
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.GET,
                Url = StaticDetails.ProductAPIBase + $"/api/product"
            });
        }

        public async Task<ResponseDto?> UpdateProduct(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = ApiType.PUT,
                Url = StaticDetails.ProductAPIBase + $"/api/product",
                Data = productDto
            });
        }
    }
}
