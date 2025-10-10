using MediatR;

namespace VehicleShowroomManagement.Application.Features.Users.Commands.CreateUser
{
    /// <summary>
    /// Command to create a new user (unified schema)
    /// </summary>
    public record CreateUserCommand(
        string Username,
        string Email,
        string Password,
        string Name,
        string RoleId,
        string? Phone = null,
        string? Address = null,
        DateTime? HireDate = null)
        : IRequest<string>;
}
