using MediatR;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Application.Features.Users.Commands.CreateUser
{
    /// <summary>
    /// Command to create a new user
    /// </summary>
    public record CreateUserCommand : IRequest<string>
    {
        public string Username { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public UserRole Role { get; init; }
        public string? Phone { get; init; }

        public CreateUserCommand(string username, string email, string password, string firstName, string lastName, UserRole role, string? phone = null)
        {
            Username = username;
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Role = role;
            Phone = phone;
        }
    }
}