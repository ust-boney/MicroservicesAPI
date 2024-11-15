using Mango.Web.Models;
using Mango.Web.Services.IService;
using Mango.Web.Utility;

namespace Mango.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;

        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StandardUtility.ApiType.POST,
                Url = StandardUtility.AuthAPIBase + "/api/auth/login",
                Data = loginRequestDto,
            });
        }

        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StandardUtility.ApiType.POST,
                Url = StandardUtility.AuthAPIBase + "/api/auth/register",
                Data = registrationRequestDto,
            });
        }

        public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto assignRoleRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StandardUtility.ApiType.POST,
                Url = StandardUtility.AuthAPIBase + "/api/auth/assignrole",
                Data = assignRoleRequestDto,
            });
        }
    }
}
