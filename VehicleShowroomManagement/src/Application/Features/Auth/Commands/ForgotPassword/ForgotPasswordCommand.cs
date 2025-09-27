using MediatR;

namespace VehicleShowroomManagement.Application.Features.Auth.Commands.ForgotPassword
{
    /// <summary>
    /// Command for initiating password reset process
    /// </summary>
    public record ForgotPasswordCommand(string Email) : IRequest;
}