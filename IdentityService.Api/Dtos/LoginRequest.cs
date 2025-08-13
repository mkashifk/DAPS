using System.Text.Json.Serialization;

namespace IdentityService.Api.Dtos
{
    public class LoginRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;

        [JsonPropertyName("password")]
        public string Password { get; set; } = null!;
    }
}
