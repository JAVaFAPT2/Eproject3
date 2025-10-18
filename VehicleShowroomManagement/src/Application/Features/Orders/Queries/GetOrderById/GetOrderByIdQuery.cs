using MediatR;

namespace VehicleShowroomManagement.Application.Features.Orders.Queries.GetOrderById
{
    /// <summary>
    /// Query to get a single order by ID
    /// </summary>
    public record GetOrderByIdQuery(string OrderId) : IRequest<OrderDetailDto?>;
}
