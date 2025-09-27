using MediatR;

namespace VehicleShowroomManagement.Application.Features.Auth.Commands.ResetPassword
{
    /// <summary>
    /// Command for resetting password using reset token
    /// </summary>
    public record ResetPasswordCommand(string Token, string NewPassword) : IRequest<bool>;
}