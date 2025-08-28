using IdentityService.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Models;
using SharedKernel.Security;
using IdentityService.Api.Dtos;


namespace IdentityService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IdentityDbContext _context;
        private readonly IConfiguration _configuration;

        public UserController(IdentityDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET api/user
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();

            return Ok(users);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
        {
            // Validate tenant existence (optional)
            // var tenantExists = await _context.Tenants.AnyAsync(t => t.TenantId == request.TenantId);
            // if (!tenantExists)
            // Hash password
            var passwordHash = PasswordHasher.HashPassword(request.Password);

            var user = new User
            {
                UserId = Guid.NewGuid(),
                TenantId = request.TenantId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = passwordHash,
                DepartmentId = request.DepartmentId,
                DesignationId = request.DesignationId,
                RoleId = request.RoleId,
                StatusId = request.StatusId,
                EntityId = request.EntityId,
                BranchId = request.BranchId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.CreatedBy, // Admin or superuser making request
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = request.CreatedBy
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            //var response = new ApiResponse<User>(user, 200, "User registered successfully");
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<string?>(null, 400, "Invalid request data"));

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                return Unauthorized(new ApiResponse<string?>(null, 401, "Invalid email or password"));

            bool isPasswordValid = PasswordHasher.VerifyPassword(request.Password, user.PasswordHash);
            if (!isPasswordValid)
                return Unauthorized(new ApiResponse<string?>(null, 401, "Invalid email or password"));

            // JWT expiry
            var expirySetting = _configuration["JwtSettings:ExpiryMinutes"];
            var expiryMinutes = int.TryParse(expirySetting, out var minutes) ? minutes : 60;

            var jwtToken = JwtTokenGenerator.GenerateToken(
                user.UserId, user.Email,
                _configuration["JwtSettings:SecretKey"] ?? throw new InvalidOperationException("Missing SecretKey"),
                _configuration["JwtSettings:Issuer"] ?? "defaultIssuer",
                _configuration["JwtSettings:Audience"] ?? "defaultAudience",
                expiryMinutes
            );

            // Generate refresh token
            var refreshToken = TokenService.GenerateRefreshToken();

            // Save refresh token in DB
            var entity = new RefreshToken
            {
                UserId = user.UserId,
                Token = refreshToken.Token,
                ExpiresAt = refreshToken.ExpiresAt,
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };

            _context.RefreshTokens.Add(entity);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>(new
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                User = new
                {
                    user.UserId,
                    user.Email
                }
            }, 200, "Login successful"));
        }


    }
}
