using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityService.Api.Entities;
using IdentityService.Api.DTOs;
using IdentityService.Api.Services;

namespace IdentityService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IdentityDbContext _context;
        private readonly TokenService _tokenService;

        public AuthController(IdentityDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var existingToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken);

            if (existingToken == null || existingToken.ExpiresAt <= DateTime.UtcNow)
                return Unauthorized(new { message = "Invalid or expired refresh token" });

            var user = await _context.Users.FindAsync(existingToken.UserId);
            if (user == null)
                return Unauthorized(new { message = "User not found" });

            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken(user.UserId);

            _context.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token
            });
        }
    }
}
