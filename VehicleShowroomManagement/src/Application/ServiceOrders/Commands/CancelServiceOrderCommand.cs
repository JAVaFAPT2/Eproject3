using MediatR;

namespace VehicleShowroomManagement.Application.ServiceOrders.Commands
{
    /// <summary>
    /// Command to cancel a service order
    /// </summary>
    public class CancelServiceOrderCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
        public string? CancellationReason { get; set; }
    }
}
