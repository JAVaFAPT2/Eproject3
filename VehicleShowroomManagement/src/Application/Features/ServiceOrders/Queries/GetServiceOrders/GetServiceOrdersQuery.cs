using MediatR;

namespace VehicleShowroomManagement.Application.Features.ServiceOrders.Queries.GetServiceOrders
{
    /// <summary>
    /// Query to get service orders with pagination and filtering
    /// </summary>
    public record GetServiceOrdersQuery(
        int PageNumber = 1,
        int PageSize = 10,
        string? Status = null,
        string? OrderId = null) : IRequest<ServiceOrdersResponse>;
}
