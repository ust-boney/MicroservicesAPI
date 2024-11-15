using Mango.Web.Models;
using Mango.Web.Services.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService  authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
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
               
                await SignInUser(loginResponseDto);
                _tokenProvider.SetToken(loginResponseDto.AccessToken);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = responseDto.Message;
                return View(model);
            }
            
           
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

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(LoginResponseDto model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.ReadJwtToken(model.AccessToken);
           
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, 
                jwt.Claims.FirstOrDefault(u=>u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role,
               jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
