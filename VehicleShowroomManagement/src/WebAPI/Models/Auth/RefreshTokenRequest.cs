namespace VehicleShowroomManagement.WebAPI.Models.Auth
{
    /// <summary>
    /// Request model for refresh token
    /// </summary>
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}