using MediatR;

namespace VehicleShowroomManagement.Application.ServiceOrders.Commands
{
    /// <summary>
    /// Command to complete a service order
    /// </summary>
    public class CompleteServiceOrderCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
        public string? CompletionNotes { get; set; }
    }
}
