using Mango.Web.Models;
using Mango.Web.Services;
using Mango.Web.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            List<ProductDto> productList = new();
            ResponseDto? responseDto = await _productService.GetAllProductsAsync();
            if (responseDto != null && responseDto.IsSuccess) {
                productList =  JsonConvert.DeserializeObject<List<ProductDto>>(responseDto.Result.ToString());
            }
            else
            {
                TempData["error"] = responseDto?.Message;
            }
            return View(productList);
        }

        [HttpGet]
        public async Task<IActionResult> ProductCreate(int? productId=0)
        {
            ProductDto model = new();
            if (productId > 0)
            {
                ResponseDto? responseDto = await _productService.GetProductsByIdAsync((int)productId);
                model= JsonConvert.DeserializeObject<ProductDto>(responseDto.Result.ToString());
                
            }
           
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? responseDto = null;
                if (model.ProductId > 0)
                {
                    responseDto = await _productService.UpdateProductsAsync(model);
                }
                else
                {
                    responseDto = await _productService.CreateProductsAsync(model);
                }

                if (responseDto != null && responseDto.IsSuccess)
                {
                    TempData["success"] = model.ProductId == 0 ? "Product created successfully!" : "Product updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                {
                    TempData["error"] = responseDto?.Message;
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ProductDelete(int productId)
        {
            ResponseDto? responseDto = await _productService.GetProductsByIdAsync(productId);
            if (responseDto != null && responseDto.IsSuccess)
            {
                ProductDto model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responseDto.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = responseDto?.Message;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto model)
        {
            ResponseDto? responseDto= await _productService.DeleteProductsAsync(model.ProductId);
            if(responseDto != null && responseDto.IsSuccess)
            {
                TempData["success"] = "Product deleted successfully!";
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
