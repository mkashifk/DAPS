using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace IdentityService.Api.Dtos
{
    public class RegisterUserRequest
    {
        [Required]
        [JsonPropertyName("tenant_id")]
        public Guid TenantId { get; set; }

        [Required]
        [MaxLength(100)]
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [JsonPropertyName("last_name")]
        public string LastName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [JsonPropertyName("password")]
        public string Password { get; set; } = null!; // Will be hashed

        [JsonPropertyName("department_id")]
        public Guid? DepartmentId { get; set; }

        [JsonPropertyName("designation_id")]
        public Guid? DesignationId { get; set; }

        [JsonPropertyName("role_id")]
        public Guid? RoleId { get; set; }

        [JsonPropertyName("status_id")]
        public Guid? StatusId { get; set; }

        [JsonPropertyName("entity_id")]
        public Guid? EntityId { get; set; }

        [JsonPropertyName("branch_id")]
        public Guid? BranchId { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonPropertyName("created_by")]
        public Guid CreatedBy { get; set; } // Set by backend (admin's ID)
    }
}
