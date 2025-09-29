using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.WebAPI.Models.SalesOrders
{
    /// <summary>
    /// Request model for starting an order (only status update)
    /// </summary>
    public class StartOrderRequest
    {
        public string CustomerId { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public OrderStatus InitialStatus { get; set; } = OrderStatus.Pending;
    }
}
