using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Features.SalesOrders.Queries.GetOrderById
{
    /// <summary>
    /// Query for getting sales order by ID
    /// </summary>
    public record GetOrderByIdQuery(string Id) : IRequest<OrderDto?>;
}