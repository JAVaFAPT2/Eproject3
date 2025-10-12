namespace VehicleShowroomManagement.Application.Common.Models
{
    /// <summary>
    /// Result DTO for UpdateServiceOrderStatus operation
    /// </summary>
    public class UpdateServiceOrderStatusResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? BillingDocumentId { get; set; }
    }
}

