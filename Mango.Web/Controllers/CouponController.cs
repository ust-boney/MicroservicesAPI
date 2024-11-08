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
            if (responseDto != null && responseDto.IsSuccess)
            {
                listCouponDto = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(responseDto.Result));
            }
            else
            {
                TempData["error"] = responseDto?.Message;
            }
         
            return View(listCouponDto);
        }

        [HttpGet]
        public async Task<IActionResult> CouponCreate()
        {

          return View();
        }

        [HttpPost] 
        public async Task<IActionResult> CouponCreate(CouponDto model) 
        {
            if (ModelState.IsValid)
            {
                ResponseDto responseDto = await _couponService.CreateCouponsAsync(model);
                if (responseDto!=null && responseDto.IsSuccess)
                {
                    TempData["success"] = "Coupon created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = responseDto?.Message;
                }
            }
          
            return View(model);
        }

        public async Task<IActionResult> CouponDelete(int couponId)
        {
			ResponseDto? responseDto = await _couponService.GetCouponsByIdAsync(couponId);
			if (responseDto != null && responseDto.IsSuccess)
			{
				CouponDto model = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(responseDto.Result));
                return View(model);
			}
            else
            {
                TempData["error"] = responseDto?.Message;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto model)
        {
			ResponseDto responseDto = await _couponService.DeleteCouponsAsync(model.CouponId);
			if (responseDto != null && responseDto.IsSuccess)
			{
                TempData["success"] = "Coupon deleted successfully!";
                return RedirectToAction(nameof(Index));
			}
            else
            {
                TempData["error"] = responseDto?.Message;
            }

            return View(model);
		}
    }
}
