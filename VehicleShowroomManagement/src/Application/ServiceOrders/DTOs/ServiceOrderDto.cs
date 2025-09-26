namespace VehicleShowroomManagement.Application.ServiceOrders.DTOs
{
    /// <summary>
    /// Data Transfer Object for Service Order
    /// </summary>
    public class ServiceOrderDto
    {
        public string Id { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public DateTime ServiceDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal TotalCost { get; set; }
        public string? Description { get; set; }
        public string ServiceType { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
