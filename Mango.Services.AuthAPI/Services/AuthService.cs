using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _dbcontext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenGeneratorService _tokenGeneratorService;

        public AuthService(AppDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ITokenGeneratorService tokenGeneratorService)
        {
            _dbcontext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenGeneratorService = tokenGeneratorService;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
           var user= _dbcontext.ApplicationUsers.FirstOrDefault(u=> u.UserName.ToLower()== loginRequestDto.UserName.ToLower());
            bool isValid = true;
            LoginResponseDto loginResponseDto;

            if (user != null)
            {
              isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            }

            if (user == null || !isValid)
            {
                return new LoginResponseDto() { User = null, AccessToken = "" };
            }
            else
            {
                // user and password is valid
                // if user was found generate token
                string token = _tokenGeneratorService.GenerateToken(user);
                UserDto userDto = new UserDto()
                {
                    Email = user.UserName,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                    ID = user.Id
                };
                loginResponseDto = new()
                {
                    User = userDto,
                    AccessToken = token
                };
               
            }
            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser applicationUser = new()
            {
                UserName= registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                Name = registrationRequestDto.Name,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                PhoneNumber = registrationRequestDto.PhoneNumber,
            };

            try
            {
                var result= await _userManager.CreateAsync(applicationUser, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    var user= _dbcontext.Users.First(u=>u.Email==registrationRequestDto.Email);
                    UserDto userDto = new()
                    {
                        Email = user.Email,
                        Name = user.Name,
                        PhoneNumber = user.PhoneNumber,
                        ID = user.Id,
                    }; 

                    return "";
                }
                else
                {
                  return  result.Errors.FirstOrDefault().Description;
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            
        }

        async Task<bool> IAuthService.AssignRole(string email, string roleName)
        {
            var user = _dbcontext.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == email.ToLower());
            if (user != null)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }

            return false;
        }
    }
}
