using MediatR;

namespace VehicleShowroomManagement.Application.Features.Users.Commands.CreateUser
{
    /// <summary>
    /// Command to create a new user (unified schema)
    /// RoleId is optional - if not provided, role will be auto-assigned based on HireDate
    /// </summary>
    public record CreateUserCommand(
        string Username,
        string Email,
        string Password,
        string Name,
        string? RoleId = null,
        string? Phone = null,
        string? Address = null,
        DateTime? HireDate = null)
        : IRequest<string>;
}
