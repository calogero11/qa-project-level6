using Newtonsoft.Json;

namespace webapi.Models.Responses;

public class AuthInfo
{
    [JsonProperty("userGuid")]
    public Guid UserGuid { get; set; }
        
    [JsonProperty("email")]
    public string Email { get; set; }
    
    [JsonProperty("roles")]
    public IEnumerable<string> Roles { get; set; }
}