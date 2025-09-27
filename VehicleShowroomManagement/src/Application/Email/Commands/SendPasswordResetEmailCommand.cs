using MediatR;

namespace VehicleShowroomManagement.Application.Email.Commands
{
    /// <summary>
    /// Command to send password reset email
    /// </summary>
    public class SendPasswordResetEmailCommand : IRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ResetToken { get; set; } = string.Empty;
        public int ExpiryHours { get; set; } = 24;
        public string BaseUrl { get; set; } = string.Empty;
    }
}
