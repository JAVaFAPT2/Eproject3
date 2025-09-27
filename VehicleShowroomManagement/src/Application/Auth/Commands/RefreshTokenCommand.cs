using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Auth.Commands
{
    /// <summary>
    /// Command for refreshing JWT token using refresh token
    /// </summary>
    public class RefreshTokenCommand : IRequest<LoginResultDto>
    {
        public string RefreshToken { get; set; } = string.Empty;
        public string? IpAddress { get; set; }
    }
}
