using Mango.Web.Models;
using Mango.Web.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
       
        private readonly IProductService _productService;
        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto> productList = new();
            ResponseDto? responseDto = await _productService.GetAllProductsAsync();
            if (responseDto != null && responseDto.IsSuccess)
            {
                productList = JsonConvert.DeserializeObject<List<ProductDto>>(responseDto.Result.ToString());
            }
            else
            {
                TempData["error"] = responseDto?.Message;
            }
            return View(productList);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ProductDetails(int productId)
        {
            ProductDto model = new();
            ResponseDto? responseDto = await _productService.GetProductsByIdAsync((int)productId);
            if (responseDto != null && responseDto.IsSuccess)
            {
               model = JsonConvert.DeserializeObject<ProductDto>(responseDto.Result.ToString());
            }
            else
            {
               TempData["error"] = responseDto?.Message;
            }
            return View(model);
        }

        [HttpPost]
        public void AddToCart()
        {
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
