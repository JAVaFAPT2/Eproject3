using System.Collections.Generic;
using MediatR;
using VehicleShowroomManagement.Application.DTOs;

namespace VehicleShowroomManagement.Application.Queries
{
    /// <summary>
    /// Query for getting user profile
    /// </summary>
    public class GetUserProfileQuery : IRequest<UserProfileDto>
    {
        public string UserId { get; set; }

        public GetUserProfileQuery(string userId)
        {
            UserId = userId;
        }
    }
}