using MediatR;

namespace VehicleShowroomManagement.Application.Features.Users.Commands.UpdateUserProfile
{
    /// <summary>
    /// Command to update user profile (unified schema)
    /// </summary>
    public record UpdateUserProfileCommand : IRequest
    {
        public string UserId { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
        public string? Phone { get; init; }
        public string? Address { get; init; }

        public UpdateUserProfileCommand(string userId, string name, string email, string? phone = null, string? address = null)
        {
            UserId = userId;
            Name = name;
            Email = email;
            Phone = phone;
            Address = address;
        }
    }
}
