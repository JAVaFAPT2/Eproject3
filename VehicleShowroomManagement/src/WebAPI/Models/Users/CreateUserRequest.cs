namespace VehicleShowroomManagement.WebAPI.Models.Users
{
    /// <summary>
    /// Request model for creating a user (unified schema)
    /// RoleId is optional - if not provided, role will be auto-assigned based on HireDate
    /// (Employee if HireDate is provided, Customer otherwise)
    /// </summary>
    public class CreateUserRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? RoleId { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public DateTime? HireDate { get; set; }
    }
}
