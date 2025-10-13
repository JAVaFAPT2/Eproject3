using MediatR;
using VehicleShowroomManagement.Application.Features.Users.Queries.GetUserById;

namespace VehicleShowroomManagement.Application.Features.Users.Queries.GetUsersByRole
{
    /// <summary>
    /// Query to get all users filtered by role name
    /// </summary>
    public record GetUsersByRoleQuery(string RoleName) : IRequest<List<UserDto>>;
}
