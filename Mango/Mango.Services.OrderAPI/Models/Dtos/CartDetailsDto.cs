
namespace Mango.Services.OrderAPI.Models.Dtos
{
    public class CartDetailsDto
    {
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public ProductDto? Product { get; set; }
        public CartHeaderDto? CartHeader { get; set; }
    }
}
