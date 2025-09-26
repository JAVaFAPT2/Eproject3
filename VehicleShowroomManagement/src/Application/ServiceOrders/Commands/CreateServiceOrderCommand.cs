using MediatR;

namespace VehicleShowroomManagement.Application.ServiceOrders.Commands
{
    /// <summary>
    /// Command to create a new service order
    /// </summary>
    public class CreateServiceOrderCommand : IRequest<string>
    {
        public string VehicleId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public DateTime ServiceDate { get; set; }
        public string ServiceType { get; set; } = string.Empty;
        public decimal TotalCost { get; set; } = 0;
        public string? Description { get; set; }
    }
}
