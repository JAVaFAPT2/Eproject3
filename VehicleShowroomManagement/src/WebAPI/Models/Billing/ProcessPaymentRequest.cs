using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.WebAPI.Models.Billing
{
    /// <summary>
    /// Request model for processing payment
    /// </summary>
    public class ProcessPaymentRequest
    {
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string? PaymentReference { get; set; }
        public string ProcessedBy { get; set; } = string.Empty;
    }
}