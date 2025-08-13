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

        public UserController(IdentityDbContext context)
        {
            _context = context;
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
            var hashedPassword = PasswordHasher.HashPassword(request.Password);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email && u.PasswordHash == hashedPassword);

            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            // TODO: Generate JWT token here and return in response if applicable

            return Ok(user);
        }

    }
}
