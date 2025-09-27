using MediatR;

namespace VehicleShowroomManagement.Application.Auth.Commands
{
    /// <summary>
    /// Command for reset password functionality
    /// </summary>
    public class ResetPasswordCommand : IRequest
    {
        public string Token { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
}
