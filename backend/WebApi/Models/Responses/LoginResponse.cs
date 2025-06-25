using Newtonsoft.Json;

namespace WebApi.Models.Responses;

public class LoginResponse
{
    [JsonProperty("accessToken")]
    public string? AccessToken { get; set; }
}