namespace VehicleShowroomManagement.Application.Common.DTOs
{
    /// <summary>
    /// DTO for order printing
    /// </summary>
    public class OrderPrintDto
    {
        public string OrderNumber { get; set; } = string.Empty;
        public CustomerInfo Customer { get; set; } = new CustomerInfo();
        public VehicleInfo Vehicle { get; set; } = new VehicleInfo();
        public UserInfo SalesPerson { get; set; } = new UserInfo();
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public string CompanyInfo { get; set; } = "Vehicle Showroom Management System";
    }
}


