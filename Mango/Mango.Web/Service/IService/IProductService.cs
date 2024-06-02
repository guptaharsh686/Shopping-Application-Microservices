using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDto?> CreateProduct(ProductDto productDto);
        Task<ResponseDto?> UpdateProduct(ProductDto productDto);
        Task<ResponseDto?> DeleteProduct(int id);
        Task<ResponseDto?> GetAllProducts();
    }
}
