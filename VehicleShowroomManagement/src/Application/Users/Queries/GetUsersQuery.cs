using System.Collections.Generic;
using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Users.Queries
{
    /// <summary>
    /// Query for retrieving users with optional filtering
    /// </summary>
    public class GetUsersQuery : IRequest<IEnumerable<UserDto>>
    {
        public string? SearchTerm { get; set; }
        public int? RoleId { get; set; }
        public bool? IsActive { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public GetUsersQuery(string? searchTerm = null, int? roleId = null, bool? isActive = null, int pageNumber = 1, int pageSize = 10)
        {
            SearchTerm = searchTerm;
            RoleId = roleId;
            IsActive = isActive;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    /// <summary>
    /// Query for retrieving a single user by ID
    /// </summary>
    public class GetUserByIdQuery : IRequest<UserDto?>
    {
        public string UserId { get; set; }

        public GetUserByIdQuery(string userId)
        {
            UserId = userId;
        }
    }
}
