using AutoMapper;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;

namespace Mango.Services.CouponAPI.Profiles
{
    public class CuponsProfile : Profile
    {
        public CuponsProfile()
        {
            //Source -> Dist
            CreateMap<Coupon, CouponDto>();
            CreateMap<CouponDto, Coupon>();

        }
    }
}
