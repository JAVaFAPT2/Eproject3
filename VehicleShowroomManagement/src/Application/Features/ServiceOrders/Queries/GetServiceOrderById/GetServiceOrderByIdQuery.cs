using MediatR;

namespace VehicleShowroomManagement.Application.Features.ServiceOrders.Queries.GetServiceOrderById
{
    public class GetServiceOrderByIdQuery : IRequest<ServiceOrderDto>
    {
        public string ServiceOrderId { get; set; } = string.Empty;
    }
}