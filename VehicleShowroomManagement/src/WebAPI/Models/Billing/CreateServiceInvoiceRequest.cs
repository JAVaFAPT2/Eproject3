namespace VehicleShowroomManagement.WebAPI.Models.Billing
{
    /// <summary>
    /// Request model for creating service invoice
    /// </summary>
    public class CreateServiceInvoiceRequest
    {
        public string ServiceOrderId { get; set; } = string.Empty;
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
        public decimal LaborCost { get; set; }
        public decimal PartsCost { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }
        public List<ServiceInvoiceLineItem> LineItems { get; set; } = new List<ServiceInvoiceLineItem>();
    }

    /// <summary>
    /// Service invoice line item
    /// </summary>
    public class ServiceInvoiceLineItem
    {
        public string Description { get; set; } = string.Empty;
        public string ItemType { get; set; } = "Labor"; // Labor, Parts, Other
        public decimal Quantity { get; set; } = 1;
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public string? PartNumber { get; set; }
    }
}