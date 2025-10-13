namespace VehicleShowroomManagement.Application.Features.ServiceOrders.Queries.GetServiceOrders
{
    /// <summary>
    /// Data Transfer Object for Service Order
    /// </summary>
    public class ServiceOrderDto
    {
        public string Id { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? ServiceDate { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string? Description { get; set; }
        public decimal Cost { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response model for paginated service orders
    /// </summary>
    public class ServiceOrdersResponse
    {
        public List<ServiceOrderDto> ServiceOrders { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
