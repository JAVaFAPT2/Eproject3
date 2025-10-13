using MediatR;

namespace VehicleShowroomManagement.Application.Features.Users.Commands.UpdateUserActive
{
    /// <summary>
    /// Command to update user's active status only
    /// </summary>
    public record UpdateUserActiveCommand(string UserId, bool IsActive) : IRequest;
}


