using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.WebAPI.Models.SalesOrders
{
    /// <summary>
    /// Enhanced request model for updating order status
    /// </summary>
    public class UpdateOrderStatusRequest
    {
        public OrderStatus Status { get; set; }
        public string? StatusDescription { get; set; }
        public string? Notes { get; set; }
        public string? InternalNotes { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
        public int Priority { get; set; } = 1; // 1=Low, 2=Medium, 3=High
        public bool IsUrgent { get; set; } = false;
        public string UpdatedBy { get; set; } = string.Empty;
    }
}