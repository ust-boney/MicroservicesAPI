using Mango.Web.Models;
using Mango.Web.Services.IService;
using Mango.Web.Utility;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static Mango.Web.Utility.StandardUtility;

namespace Mango.Web.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory; 
        private readonly ITokenProvider _tokenProvider;

        public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
        {
            _httpClientFactory = httpClientFactory;
            _tokenProvider = tokenProvider;
        }
        public async Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearerToken = true)
        {
            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient("Mango");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
               
                if (withBearerToken)
                {
                    string? token = _tokenProvider.GetToken();
                    message.Headers.Add("Authorization", $"Bearer {token}");
                }
                message.RequestUri = new Uri(requestDto.Url);
                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage apiResponse = null;
                switch (requestDto.ApiType)
                {
                    case ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                apiResponse = await httpClient.SendAsync(message);
                switch (apiResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new ResponseDto() { IsSuccess = false, Message = "Not Found" };
                    case HttpStatusCode.Unauthorized:
                        return new ResponseDto() { IsSuccess = false, Message = "User not authorized" };
                    case HttpStatusCode.Forbidden:
                        return new ResponseDto() { IsSuccess = false, Message = "Access denied" };
                    case HttpStatusCode.InternalServerError:
                        return new ResponseDto() { IsSuccess = false, Message = "Internal Server Error" };

                    default:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        var content = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

                        return content;
                }
            }
            catch (Exception ex)
            {
                return new ResponseDto() { IsSuccess = false,Message=ex.Message };
            }
 
            
        }
    }
}
