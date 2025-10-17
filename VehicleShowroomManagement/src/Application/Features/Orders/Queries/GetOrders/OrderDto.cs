namespace VehicleShowroomManagement.Application.Features.Orders.Queries.GetOrders
{
    /// <summary>
    /// Data Transfer Object for Order
    /// </summary>
    public class OrderDto
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
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// Response model for paginated orders
    /// </summary>
    public class OrdersResponse
    {
        public List<OrderDto> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
