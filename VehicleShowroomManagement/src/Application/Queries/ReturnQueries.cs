using System.Collections.Generic;
using MediatR;
using VehicleShowroomManagement.Application.DTOs;

namespace VehicleShowroomManagement.Application.Queries
{
    /// <summary>
    /// Query for getting return requests with pagination
    /// </summary>
    public class GetReturnsQuery : IRequest<IEnumerable<ReturnRequestDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public GetReturnsQuery(int pageNumber = 1, int pageSize = 10)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}