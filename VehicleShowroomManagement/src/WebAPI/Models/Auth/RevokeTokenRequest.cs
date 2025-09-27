namespace VehicleShowroomManagement.WebAPI.Models.Auth
{
    /// <summary>
    /// Request model for revoke token
    /// </summary>
    public class RevokeTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}