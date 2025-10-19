namespace VehicleShowroomManagement.Application.Features.Orders.Queries.GetOrderById
{
    /// <summary>
    /// Detailed Order DTO with additional information
    /// </summary>
    public class OrderDetailDto
    {
        public string Id { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string DealerId { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;
        public decimal SalePrice { get; set; }
        public string? VehicleId { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string? Note { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public DateTime? ReservationFrom { get; set; }
        public DateTime? ReservationTo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
