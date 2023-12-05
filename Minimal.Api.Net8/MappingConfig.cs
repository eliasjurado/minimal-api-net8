using AutoMapper;
using Minimal.Api.Net8.Models;
using Minimal.Api.Net8.Models.DTO;

namespace Minimal.Api.Net8
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Coupon, CouponRequestDTO>().ReverseMap();
            CreateMap<Coupon, CouponDTO>().ReverseMap();
        }
    }
}
