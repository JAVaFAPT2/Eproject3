using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.WebAPI.Models.SalesOrders
{
    /// <summary>
    /// Request model for creating a sales order
    /// </summary>
    public class CreateSalesOrderRequest
    {
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public string SalesPersonId { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}