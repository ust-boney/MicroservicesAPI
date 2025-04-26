using AutoMapper;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ShoppingCartAPI.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ResponseDto _responseDto;

        public CartAPIController(AppDbContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDto cartDto)
        {
           
            try
            {
                var cartHeaderFromDB = await _dbContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);
                if (cartHeaderFromDB == null)
                {
                    // cart is empty add new record in cart header and cart details
                    CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                    _dbContext.CartHeaders.Add(cartHeader);
                    await _dbContext.SaveChangesAsync();

                    cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    CartDetails cartDetails = _mapper.Map<CartDetails>(cartDto.CartDetails.First());
                    _dbContext.CartDetails.Add(cartDetails);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    var cartDetailFromDb = await _dbContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(u => u.ProductId == cartDto.CartDetails.First().ProductId
                    && u.CartHeader.CartHeaderId == cartHeaderFromDB.CartHeaderId);
                    if (cartDetailFromDb == null)
                    {
                        // need to add entry in cart details cart header exists
                        CartDetails cartDetails = _mapper.Map<CartDetails>(cartDto.CartDetails.First());
                        _dbContext.CartDetails.Add(cartDetails);
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        // cart exists with the same product and need to update the product count
                        cartDto.CartDetails.First().Count += cartDetailFromDb.Count;
                        cartDto.CartDetails.First().CartHeaderId = cartDetailFromDb.CartHeaderId;
                        cartDto.CartDetails.First().CartDetailId= cartDetailFromDb.CartDetailId;
                        _dbContext.Update(_mapper.Map<CartDetails>(cartDto.CartDetails.First()));
                        await _dbContext.SaveChangesAsync();
                    }
                }
                _responseDto.Result = cartDto;
                _responseDto.IsSuccess= true;
            }
            catch (Exception ex)
            {
                _responseDto.Message= ex.Message.ToString();
                _responseDto.IsSuccess= false;
            }
            return _responseDto;
        }

        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody] int cartDetailID)
        {
            try
            {
                // first fetch cart detail with cartdetailID and remove item from cartdetail table
                // Check how many items are present in cartdetail with the same headerId of item to be removed
                // if there is only one item with CartHeaderId then remove the item from CartHeader table

                CartDetails cartDetails = _dbContext.CartDetails.First
                    (u => u.CartDetailId == cartDetailID);
                _dbContext.CartDetails.Remove(cartDetails);
                if (cartDetails != null)
                {

                    // cart is empty add new record in cart header and cart details
                    //CartHeader cartHeader = _mapper.Map<CartHeader>(cartDto.CartHeader);
                    //_dbContext.CartHeaders.Add(cartHeader);
                    //await _dbContext.SaveChangesAsync();

                    //cartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    //CartDetails cartDetails = _mapper.Map<CartDetails>(cartDto.CartDetails.First());
                    //_dbContext.CartDetails.Add(cartDetails);
                    //await _dbContext.SaveChangesAsync();
                }

                //_responseDto.Result = cartDto;
                //_responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message.ToString();
                _responseDto.IsSuccess = false;
            }
            return _responseDto;
        }
    }
}
