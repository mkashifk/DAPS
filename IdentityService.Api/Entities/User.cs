using System;
using System.Collections.Generic;

namespace IdentityService.Api.Entities;

public partial class User
{
    public Guid UserId { get; set; }

    public Guid TenantId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public Guid? DepartmentId { get; set; }

    public Guid? DesignationId { get; set; }

    public Guid? RoleId { get; set; }

    public Guid? StatusId { get; set; }

    public Guid? EntityId { get; set; }

    public Guid? BranchId { get; set; }
}
