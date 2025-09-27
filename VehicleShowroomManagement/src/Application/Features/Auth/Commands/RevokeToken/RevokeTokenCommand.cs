using MediatR;

namespace VehicleShowroomManagement.Application.Features.Auth.Commands.RevokeToken
{
    /// <summary>
    /// Command for revoking refresh token
    /// </summary>
    public record RevokeTokenCommand(string RefreshToken) : IRequest;
}