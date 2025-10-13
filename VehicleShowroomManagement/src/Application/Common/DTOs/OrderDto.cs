namespace VehicleShowroomManagement.Application.Common.DTOs
{
    /// <summary>
    /// Data Transfer Object for Sales Orders
    /// </summary>
    public class OrderDto
    {
        public string Id { get; set; } = string.Empty;
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public CustomerInfo Customer { get; set; } = new CustomerInfo();
        public string VehicleId { get; set; } = string.Empty;
        public string VehicleDetails { get; set; } = string.Empty;
        public VehicleInfo Vehicle { get; set; } = new VehicleInfo();
        public string SalesPersonId { get; set; } = string.Empty;
        public string SalesPersonName { get; set; } = string.Empty;
        public UserInfo SalesPerson { get; set; } = new UserInfo();
        public string Status { get; set; } = "DRAFT";
        public string StatusDescription { get; set; } = string.Empty;
        public DateTime? StatusUpdatedAt { get; set; }
        public string? StatusUpdatedBy { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public string? Notes { get; set; }
        public string? InternalNotes { get; set; }
        public int Priority { get; set; } = 1; // 1=Low, 2=Medium, 3=High
        public bool IsUrgent { get; set; } = false;
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }


    
}
