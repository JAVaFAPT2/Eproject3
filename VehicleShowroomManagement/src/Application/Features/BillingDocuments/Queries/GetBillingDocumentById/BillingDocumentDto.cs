using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.BillingDocuments.Queries.GetBillingDocumentById
{
    public class BillingDocumentDto
    {
        public string Id { get; set; } = string.Empty;
        public string BillId { get; set; } = string.Empty;
        public string BillNumber { get; set; } = string.Empty;
        public string SalesOrderId { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public DateTime BillDate { get; set; }
        public decimal Amount { get; set; }
        public BillOutputDto? Output { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static BillingDocumentDto FromEntity(BillingDocument billingDocument)
        {
            return new BillingDocumentDto
            {
                Id = billingDocument.Id,
                BillId = billingDocument.BillId,
                BillNumber = billingDocument.BillNumber,
                SalesOrderId = billingDocument.SalesOrderId,
                EmployeeId = billingDocument.EmployeeId,
                BillDate = billingDocument.BillDate,
                Amount = billingDocument.Amount,
                Output = billingDocument.Output != null ? BillOutputDto.FromEntity(billingDocument.Output) : null,
                CreatedAt = billingDocument.CreatedAt,
                UpdatedAt = billingDocument.UpdatedAt
            };
        }
    }

    public class BillOutputDto
    {
        public string Format { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
        public string Url { get; set; } = string.Empty;

        public static BillOutputDto FromEntity(BillingDocument.BillOutput output)
        {
            return new BillOutputDto
            {
                Format = output.Format,
                GeneratedAt = output.GeneratedAt,
                Url = output.Url
            };
        }
    }
}