using System.Security.Cryptography;
using IdentityService.Api.Entities;
using SharedKernel.Security;

namespace IdentityService.Api.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public string GenerateAccessToken(User user)
        {
            var expirySetting = _configuration["JwtSettings:ExpiryMinutes"];
            var expiryMinutes = int.TryParse(expirySetting, out var minutes) ? minutes : 60;

            var jwtToken = JwtTokenGenerator.GenerateToken(
                user.UserId, user.Email,
                _configuration["JwtSettings:SecretKey"] ?? throw new InvalidOperationException("Missing SecretKey"),
                _configuration["JwtSettings:Issuer"] ?? "defaultIssuer",
                _configuration["JwtSettings:Audience"] ?? "defaultAudience",
                expiryMinutes
            );
            return jwtToken;
        }

        public RefreshToken GenerateRefreshToken(Guid userId)
        {
            return new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                UserId = userId
            };
        }
    }
}
