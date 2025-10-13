namespace VehicleShowroomManagement.Application.Features.BillingDocuments.Queries.GetBillingDocuments
{
    /// <summary>
    /// Data Transfer Object for Billing Document
    /// </summary>
    public class BillingDocumentDto
    {
        public string Id { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        
        // Computed properties from domain entity
        public bool IsUnpaid { get; set; }
        public bool IsPartiallyPaid { get; set; }
        public bool IsPaid { get; set; }
    }

    /// <summary>
    /// Response model for paginated billing documents
    /// </summary>
    public class BillingDocumentsResponse
    {
        public List<BillingDocumentDto> BillingDocuments { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
