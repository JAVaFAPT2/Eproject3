namespace VehicleShowroomManagement.WebAPI.Models.SalesOrders
{
    /// <summary>
    /// Request model for updating order priority
    /// </summary>
    public class UpdateOrderPriorityRequest
    {
        public int Priority { get; set; } = 1; // 1=Low, 2=Medium, 3=High
        public bool IsUrgent { get; set; } = false;
        public string? Reason { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
    }
}