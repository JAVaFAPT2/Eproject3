using System.Collections.Generic;
using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Users.Queries
{
    /// <summary>
    /// Query for getting customers with search and pagination
    /// </summary>
    public class GetCustomersQuery : IRequest<IEnumerable<CustomerDto>>
    {
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public GetCustomersQuery(string? searchTerm = null, int pageNumber = 1, int pageSize = 10)
        {
            SearchTerm = searchTerm;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    /// <summary>
    /// Query for getting orders by customer
    /// </summary>
    public class GetCustomerOrdersQuery : IRequest<IEnumerable<OrderDto>>
    {
        public string CustomerId { get; set; }

        public GetCustomerOrdersQuery(string customerId)
        {
            CustomerId = customerId;
        }
    }
}
