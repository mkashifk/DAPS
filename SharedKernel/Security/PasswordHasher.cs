using System.Text;
using System.Security.Cryptography;

namespace SharedKernel.Security
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
