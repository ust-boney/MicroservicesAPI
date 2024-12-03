using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    [Authorize]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        readonly ResponseDto _responseDto;
        readonly IMapper _mapper;

        public CouponAPIController(AppDbContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _responseDto = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDto GetAllCoupons()
        {
            try
            {
                IEnumerable<Coupon> coupons = _dbContext.Coupons.ToList();
                IEnumerable<CouponDto> couponDtoList= _mapper.Map<IEnumerable<CouponDto>>(coupons);
                _responseDto.Result = couponDtoList;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message= ex.Message;
            }

            return _responseDto;
        }

        [HttpGet]
        [Route("{id}")]
        public ResponseDto GetCouponById(int id)
        {
            try
            {
                Coupon coupon = _dbContext.Coupons.FirstOrDefault(x=>x.CouponId==id);
                if (coupon == null)
                {
                    throw new Exception("Coupon not found");
                }
                CouponDto couponDto= _mapper.Map<CouponDto>(coupon);
                _responseDto.Result= couponDto;
                _responseDto.IsSuccess=true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message= ex.Message;
            }
            return _responseDto;
        }

        [HttpGet]
        [Route("GetCouponByCode/{code}")]
        public ResponseDto GetCouponBycode(string code)
        {
            try
            {
                Coupon coupon = _dbContext.Coupons.FirstOrDefault(x => x.CouponCode.ToLower() == code.ToLower());
                if (coupon == null)
                {
                    throw new Exception("Coupon not found");
                }
                CouponDto couponDto = _mapper.Map<CouponDto>(coupon);
                _responseDto.Result = couponDto;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ResponseDto AddCoupon([FromBody] CouponDto couponDto)
        {
            try
            {
               Coupon coupon= _mapper.Map<Coupon>(couponDto);
                _dbContext.Add(coupon);
                _dbContext.SaveChanges();
                _responseDto.Result = coupon;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ResponseDto UpdateCoupon([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon coupon = _mapper.Map<Coupon>(couponDto);
                //Coupon currentCoupon = _dbContext.Coupons.FirstOrDefault(x => x.CouponId == coupon.CouponId);
                //if (currentCoupon == null)
                //{
                //    throw new Exception("Coupon not found");
                //}

                _dbContext.Update(coupon);
                _dbContext.SaveChanges();
                _responseDto.Result = coupon;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ResponseDto DeleteCoupon(int id)
        {
            try
            {
                Coupon coupon = _dbContext.Coupons.FirstOrDefault(x => x.CouponId == id);
                if (coupon == null)
                {
                    throw new Exception("Coupon not found");
                }
                _dbContext.Coupons.Remove(coupon);
                _dbContext.SaveChanges();
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

    }
}
