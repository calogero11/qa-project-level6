using Newtonsoft.Json;

namespace webapi.Models.Responses;

public class LoginResponse
{
    [JsonProperty("accessToken")]
    public string AccessToken { get; set; }
}