using MediatR;

namespace VehicleShowroomManagement.Application.ServiceOrders.Commands
{
    /// <summary>
    /// Command to start a service order
    /// </summary>
    public class StartServiceOrderCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
    }
}
