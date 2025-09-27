using MediatR;

namespace VehicleShowroomManagement.Application.Features.Users.Commands.UpdateUserProfile
{
    /// <summary>
    /// Command to update user profile
    /// </summary>
    public record UpdateUserProfileCommand : IRequest
    {
        public string UserId { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string? Phone { get; init; }

        public UpdateUserProfileCommand(string userId, string firstName, string lastName, string? phone = null)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
        }
    }
}