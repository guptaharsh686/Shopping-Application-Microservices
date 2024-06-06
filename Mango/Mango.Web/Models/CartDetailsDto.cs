using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models
{
    public class CartDetailsDto
    {
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }

        public ProductDto? Product { get; set; }

        //navigation property        
        public CartHeaderDto? CartHeader { get; set; }
    }
}
