using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using Mango.Services.ProductAPI.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
   // [Authorize]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDbContext _dbcontext;
        private readonly IMapper _mapper;
        private readonly ResponseDto _responseDto;

        public ProductAPIController(AppDbContext dbContext, IMapper mapper)
        {
            _dbcontext = dbContext;
            _mapper = mapper;
            _responseDto = new ResponseDto();
        }

        [HttpGet]
        public ResponseDto GetAllProducts()
        {
            try
            {
                IEnumerable<Product> products = _dbcontext.Products;
                IEnumerable<ProductDto> productListDto = _mapper.Map<List<ProductDto>>(products);
                _responseDto.Result = productListDto;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess=false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpGet]
        [Route("{id}")]
        public ResponseDto GetProduct(int id)
        {
            try
            {
                Product? product = _dbcontext.Products.FirstOrDefault(x => x.ProductId == id);
                if (product == null)
                {
                    throw new Exception("Product not found");
                }
                ProductDto productDto = _mapper.Map<ProductDto>(product);
                _responseDto.Result = productDto;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess=false;
                _responseDto.Message = ex.Message;
            }
            return _responseDto;
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public ResponseDto AddProduct([FromBody] ProductDto productDto)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDto);
                _dbcontext.Products.Add(product);
                _dbcontext.SaveChanges();
                _responseDto.Result = product;
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
        public ResponseDto UpdateProduct([FromBody] ProductDto productDto)
        {
            try
            {
                Product _product = _mapper.Map<Product>(productDto);
                _dbcontext.Update(_product);
                _dbcontext.SaveChanges();
                _responseDto.Result= _product;
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
        public ResponseDto DeleteProduct(int id)
        {
            try
            {
                Product product = _dbcontext.Products.FirstOrDefault(x => x.ProductId == id);
                if (product == null)
                {
                    throw new Exception("Product not found");
                }
                _dbcontext.Products.Remove(product);
                _dbcontext.SaveChanges();
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
