using AutoMapper;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dto;

namespace Mango.Services.ShoppingCartAPI.Profiles
{
    public class ShoppingCartProfile : Profile
    {
        public ShoppingCartProfile()
        {
            //source -> dist

            CreateMap<CartHeader,CartHeaderDto>();
            CreateMap<CartDetails, CartDetailsDto>();

        }
    }
}
