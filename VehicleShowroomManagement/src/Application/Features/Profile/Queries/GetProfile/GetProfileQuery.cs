using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Features.Profile.Queries.GetProfile
{
    /// <summary>
    /// Query for getting user profile
    /// </summary>
    public record GetProfileQuery(string UserId) : IRequest<UserProfileDto?>;
}