using Mango.Web.Models;
using Mango.Web.Services.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService  authService)
        {
            _authService = authService;
        }
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto model)
        {
           var responseDto = await _authService.LoginAsync(model);
            if(responseDto!=null && responseDto.IsSuccess)
            {
                LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(responseDto.Result.ToString());
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("CustomError", responseDto?.Message);
            return View(model);
        }

        public IActionResult Register()
        {
            List<SelectListItem> roleList = new List<SelectListItem>()
            {
                new SelectListItem(){Text= StandardUtility.RoleAdmin,Value= StandardUtility.RoleAdmin},
                new SelectListItem(){Text= StandardUtility.RoleCustomer,Value= StandardUtility.RoleCustomer}
            };
            ViewBag.RoleList= roleList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto model)
        {
          var result= await _authService.RegisterAsync(model);
            if (result!=null && result.IsSuccess)
            {
                if (string.IsNullOrEmpty(model.Role))
                {
                    model.Role = StandardUtility.RoleCustomer;
                }
                var assignRoleSuccess = await _authService.AssignRoleAsync(model);
                if (assignRoleSuccess != null && assignRoleSuccess.IsSuccess)
                {
                    TempData["success"] = "User registration successful";
                }
                return RedirectToAction("Login");
            }
            else
            {
                TempData["error"] = result.Message;
                List<SelectListItem> roleList = new List<SelectListItem>()
            {
                new SelectListItem(){ Text= StandardUtility.RoleAdmin,Value= StandardUtility.RoleAdmin},
                new SelectListItem(){ Text= StandardUtility.RoleCustomer,Value= StandardUtility.RoleCustomer}
            };
                ViewBag.RoleList = roleList;
                return View(model);
            }

        }

        public IActionResult Logout()
        {
            return View();
        }
    }
}
