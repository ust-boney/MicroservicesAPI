namespace Mango.Web.Models
{
    public class LoginResponseDto
    {
        public UserDto User { get; set; }
        public string AccessToken {  get; set; }
    }
}
