using MediatR;

namespace VehicleShowroomManagement.Application.Auth.Commands
{
    /// <summary>
    /// Command for forgot password functionality
    /// </summary>
    public class ForgotPasswordCommand : IRequest
    {
        public string Email { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
    }
}
