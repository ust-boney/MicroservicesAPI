using Mango.Services.AuthAPI.Models;

namespace Mango.Services.AuthAPI.Services
{
    public interface ITokenGeneratorService
    {
       string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);
    }
}
