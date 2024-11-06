using Mango.Web.Models;
using Mango.Web.Services.IService;
using Mango.Web.Utility;

namespace Mango.Web.Services
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;

        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }
       
        public async Task<ResponseDto?> GetAllCouponsAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StandardUtility.ApiType.GET,
                Url= StandardUtility.CouponAPIBase+"/api/Coupon",
            });
        }

        public async Task<ResponseDto?> GetCouponsByCouponcodeAsync(string couponcode)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StandardUtility.ApiType.GET,
                Url = StandardUtility.CouponAPIBase + "/api/Coupon/GetCouponByCode/" + couponcode,
            });
        }

        public async Task<ResponseDto?> GetCouponsByIdAsync(int couponId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StandardUtility.ApiType.GET,
                Url = StandardUtility.CouponAPIBase + "/api/Coupon/"+couponId,
            });
        }

        public async Task<ResponseDto?> CreateCouponsAsync(CouponDto couponDto)
        {

           return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StandardUtility.ApiType.POST,
                Url = StandardUtility.CouponAPIBase + "/api/Coupon",
                Data = couponDto,
            });
        }

        public async Task<ResponseDto?> UpdateCouponsAsync(CouponDto couponDto)
        {

            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StandardUtility.ApiType.PUT,
                Url = StandardUtility.CouponAPIBase + "/api/Coupon",
                Data = couponDto,
            });
        }

        public async Task<ResponseDto?> DeleteCouponsAsync(int couponId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StandardUtility.ApiType.DELETE,
                Url = StandardUtility.CouponAPIBase + "/api/Coupon/" + couponId,
            });
        }
    }
}
