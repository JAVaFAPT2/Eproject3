namespace VehicleShowroomManagement.Application.Features.Auth.Commands.Register
{
    /// <summary>
    /// Command for public user registration (Customer role)
    /// </summary>
    public record RegisterCommand(
        string Username,
        string Password,
        string Email)
        : IRequest<string>;
}

