using System.Collections.Generic;
using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Orders.Queries
{
    /// <summary>
    /// Query for getting orders with pagination
    /// </summary>
    public class GetOrdersQuery : IRequest<IEnumerable<OrderDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public GetOrdersQuery(int pageNumber = 1, int pageSize = 10)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    /// <summary>
    /// Query for getting an order by ID
    /// </summary>
    public class GetOrderByIdQuery : IRequest<OrderDto?>
    {
        public string OrderId { get; set; }

        public GetOrderByIdQuery(string orderId)
        {
            OrderId = orderId;
        }
    }
}
