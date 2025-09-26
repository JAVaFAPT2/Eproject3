using MediatR;

namespace VehicleShowroomManagement.Application.ServiceOrders.Queries
{
    /// <summary>
    /// Query to get a service order by ID
    /// </summary>
    public class GetServiceOrderByIdQuery : IRequest<ServiceOrderDto?>
    {
        public string Id { get; set; } = string.Empty;
    }
}
