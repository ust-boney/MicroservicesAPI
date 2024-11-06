using Mango.Web.Models;
using Mango.Web.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;

        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        public async Task<IActionResult> Index()
        {
            List<CouponDto> listCouponDto = new();
           ResponseDto? responseDto= await _couponService.GetAllCouponsAsync();
            if (responseDto != null && responseDto.IsSuccess) {
               listCouponDto= JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(responseDto.Result));
            }
         
            return View(listCouponDto);
        }

        [HttpGet]
        public async Task<IActionResult> CouponCreate()
        {

          return View();
        }

        [HttpPost] 
        public async Task<IActionResult> CouponCreate(CouponDto couponDto) 
        {

            ResponseDto responseDto= await _couponService.CreateCouponsAsync(couponDto);
            if (responseDto.IsSuccess)
            {

            }
            return View();
        }
    }
}
