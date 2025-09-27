using MediatR;

namespace VehicleShowroomManagement.Application.Auth.Commands
{
    /// <summary>
    /// Command for revoking refresh token
    /// </summary>
    public class RevokeTokenCommand : IRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
        public string? IpAddress { get; set; }
    }
}
