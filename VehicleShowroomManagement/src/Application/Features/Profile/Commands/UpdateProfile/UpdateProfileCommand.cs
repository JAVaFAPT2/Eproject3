using MediatR;

namespace VehicleShowroomManagement.Application.Features.Profile.Commands.UpdateProfile
{
    /// <summary>
    /// Command for updating user profile (unified User schema)
    /// </summary>
    public record UpdateProfileCommand(
        string UserId,
        string Name,
        string Email,
        string? Phone,
        string? Address) : IRequest;
}
