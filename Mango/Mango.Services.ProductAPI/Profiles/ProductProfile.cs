using AutoMapper;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dtos;

namespace Mango.Services.ProductAPI.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            //source -> dist

            CreateMap<Product,ProductDto>();
            CreateMap<ProductDto, Product>();

        }
    }
}
