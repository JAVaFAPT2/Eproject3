using System.Collections.Generic;
using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Users.Queries
{
    /// <summary>
    /// Query for getting employees with search and pagination
    /// </summary>
    public class GetEmployeesQuery : IRequest<IEnumerable<EmployeeDto>>
    {
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public GetEmployeesQuery(string? searchTerm = null, int pageNumber = 1, int pageSize = 10)
        {
            SearchTerm = searchTerm;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    /// <summary>
    /// Query for getting employee profile
    /// </summary>
    public class GetEmployeeProfileQuery : IRequest<EmployeeProfileDto>
    {
        public string EmployeeId { get; set; } = string.Empty;

        public GetEmployeeProfileQuery(string employeeId)
        {
            EmployeeId = employeeId;
        }
    }
}
