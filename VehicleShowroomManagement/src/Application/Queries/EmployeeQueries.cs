using System.Collections.Generic;
using MediatR;
using VehicleShowroomManagement.Application.DTOs;

namespace VehicleShowroomManagement.Application.Queries
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
}