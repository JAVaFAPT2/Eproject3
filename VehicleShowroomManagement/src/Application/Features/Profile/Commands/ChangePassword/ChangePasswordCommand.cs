using MediatR;

namespace VehicleShowroomManagement.Application.Features.Profile.Commands.ChangePassword
{
    /// <summary>
    /// Command for changing user password
    /// </summary>
    public record ChangePasswordCommand(
        string UserId,
        string CurrentPassword,
        string NewPassword) : IRequest<bool>;
}