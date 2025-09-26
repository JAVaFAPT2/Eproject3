using System.Collections.Generic;
using MediatR;
using VehicleShowroomManagement.Application.DTOs;

namespace VehicleShowroomManagement.Application.Queries
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
}