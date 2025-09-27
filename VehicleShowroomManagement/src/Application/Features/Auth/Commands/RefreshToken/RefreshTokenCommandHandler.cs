using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Features.Auth.Commands.RefreshToken
{
    /// <summary>
    /// Handler for refresh token command - simplified implementation
    /// </summary>
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResultDto?>
    {
        public async Task<LoginResultDto?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            // Simplified implementation - return null for now
            // In production, implement proper refresh token validation
            await Task.CompletedTask;
            return null;
        }
    }
}