namespace VehicleShowroomManagement.Application.ServiceOrders.DTOs
{
    /// <summary>
    /// Data Transfer Object for Service Order
    /// </summary>
    public class ServiceOrderDto
    {
        public string Id { get; set; } = string.Empty;
        public string ServiceOrderId { get; set; } = string.Empty;
        public string SalesOrderId { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public DateTime ServiceDate { get; set; }
        public string? Description { get; set; }
        public decimal Cost { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
