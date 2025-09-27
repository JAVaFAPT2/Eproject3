using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Features.Auth.Commands.RefreshToken
{
    /// <summary>
    /// Command for refreshing JWT token using refresh token
    /// </summary>
    public record RefreshTokenCommand(string RefreshToken) : IRequest<LoginResultDto?>;
}