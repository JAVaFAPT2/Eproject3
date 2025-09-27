using MediatR;

namespace VehicleShowroomManagement.Application.Features.Profile.Commands.UpdateProfile
{
    /// <summary>
    /// Command for updating user profile
    /// </summary>
    public record UpdateProfileCommand(
        string UserId,
        string FirstName,
        string LastName,
        string Email,
        string? Phone) : IRequest;
}