using MediatR;
using VehicleShowroomManagement.Application.Features.Users.Queries.GetUserById;

namespace VehicleShowroomManagement.Application.Features.Users.Queries.GetUserById
{
    /// <summary>
    /// Query to get user by ID
    /// </summary>
    public record GetUserByIdQuery(string UserId) : IRequest<UserDto?>;
}