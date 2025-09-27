using MediatR;

namespace VehicleShowroomManagement.Application.Features.Auth.Commands.ResetPassword
{
    /// <summary>
    /// Handler for reset password command - simplified implementation
    /// </summary>
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            // Simplified implementation - return false for now
            // In production, implement proper password reset validation
            await Task.CompletedTask;
            return false;
        }
    }
}