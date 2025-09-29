
namespace VehicleShowroomManagement.Application.Features.Users.Commands.CreateUser
{
    /// <summary>
    /// Command to create a new user
    /// </summary>
    public record CreateUserCommand(
        string Username,
        string Email,
        string Password,
        string FirstName,
        string LastName,
        UserRole Role,
        string? Phone = null)
        : IRequest<string>;
}