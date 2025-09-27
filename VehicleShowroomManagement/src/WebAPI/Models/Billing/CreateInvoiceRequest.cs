namespace VehicleShowroomManagement.WebAPI.Models.Billing
{
    /// <summary>
    /// Request model for creating invoice
    /// </summary>
    public class CreateInvoiceRequest
    {
        public string? SalesOrderId { get; set; }
        public string? ServiceOrderId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public string InvoiceType { get; set; } = "Sales"; // Sales, Service
        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }
    }
}