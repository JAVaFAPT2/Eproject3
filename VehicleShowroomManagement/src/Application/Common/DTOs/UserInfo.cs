namespace VehicleShowroomManagement.Application.Common.DTOs
{
    /// <summary>
    /// User information for orders
    /// </summary>
    public class UserInfo
    {
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
