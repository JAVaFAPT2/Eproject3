using MediatR;

namespace VehicleShowroomManagement.Application.Features.Profile.Commands.ChangePassword
{
    /// <summary>
    /// Handler for change password command - simplified implementation
    /// </summary>
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            // Simplified implementation - return true for now
            // In production, implement proper password change with domain methods
            await Task.CompletedTask;
            return true;
        }
    }
}