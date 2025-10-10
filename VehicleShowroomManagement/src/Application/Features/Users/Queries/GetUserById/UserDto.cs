namespace VehicleShowroomManagement.Application.Features.Users.Queries.GetUserById
{
    /// <summary>
    /// Data Transfer Object for User (unified schema)
    /// </summary>
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string RoleId { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? HireDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsEmployee => HireDate.HasValue;
    }
}
