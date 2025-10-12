using MediatR;

namespace VehicleShowroomManagement.Application.Features.Auth.Commands.Register
{
    /// <summary>
    /// Command for public user registration (Customer role)
    /// </summary>
    public record RegisterCommand(
        string Username,
        string Password,
        string Email,
        string Name,
        string? Phone = null,
        string? Address = null)
        : IRequest<string>;
}

