using System.Security.Cryptography;

namespace SharedKernel.Security
{
    public class TokenService
    {
        public static (string Token, DateTime ExpiresAt) GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            var token = Convert.ToBase64String(randomBytes);
            var expiresAt = DateTime.UtcNow.AddDays(30); // adjust duration as needed

            return (token, expiresAt);
        }
    }
}
