using Mango.Web.Models;
using Mango.Web.Services.IService;
using Mango.Web.Utility;

namespace Mango.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;
        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> GetAllProductsAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StandardUtility.ApiType.GET,
                Url = StandardUtility.ProductAPIBase + "/api/products",
            });
        }

        public async Task<ResponseDto?> GetProductsByIdAsync(int productId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StandardUtility.ApiType.GET,
                Url = StandardUtility.ProductAPIBase + "/api/products/"+productId,
            });
        }

        public async Task<ResponseDto?> UpdateProductsAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StandardUtility.ApiType.PUT,
                Url = StandardUtility.ProductAPIBase + "/api/products/",
                Data = productDto
            });
        }

        public async Task<ResponseDto?> CreateProductsAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StandardUtility.ApiType.POST,
                Url = StandardUtility.ProductAPIBase + "/api/products/",
                Data = productDto
            });
        }

        public async Task<ResponseDto?> DeleteProductsAsync(int productId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StandardUtility.ApiType.DELETE,
                Url = StandardUtility.ProductAPIBase + "/api/products/"+productId,
            });
        }

      
     
    }
}
