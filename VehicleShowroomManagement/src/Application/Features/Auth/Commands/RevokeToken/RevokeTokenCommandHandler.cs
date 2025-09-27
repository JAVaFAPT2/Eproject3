using MediatR;

namespace VehicleShowroomManagement.Application.Features.Auth.Commands.RevokeToken
{
    /// <summary>
    /// Handler for revoke token command - simplified implementation
    /// </summary>
    public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand>
    {
        public async Task Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            // Simplified implementation - no-op for now
            // In production, implement proper token revocation
            await Task.CompletedTask;
        }
    }
}