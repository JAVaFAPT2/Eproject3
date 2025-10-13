using MediatR;

namespace VehicleShowroomManagement.Application.Features.Orders.Queries.GetOrders
{
    /// <summary>
    /// Query to get orders with pagination and filtering
    /// </summary>
    public record GetOrdersQuery(
        int PageNumber = 1,
        int PageSize = 10,
        string? Status = null,
        string? CustomerId = null) : IRequest<OrdersResponse>;
}
