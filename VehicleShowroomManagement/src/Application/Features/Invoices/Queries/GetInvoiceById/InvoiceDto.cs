using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.Invoices.Queries.GetInvoiceById
{
    public class InvoiceDto
    {
        public string Id { get; set; } = string.Empty;
        public string SalesOrderId { get; set; } = string.Empty;
        public string InvoiceNumber { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<string> PaymentIds { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static InvoiceDto FromEntity(Invoice invoice)
        {
            return new InvoiceDto
            {
                Id = invoice.Id,
                SalesOrderId = invoice.SalesOrderId,
                InvoiceNumber = invoice.InvoiceNumber,
                CustomerId = invoice.CustomerId,
                InvoiceDate = invoice.InvoiceDate,
                DueDate = invoice.DueDate,
                Subtotal = invoice.Subtotal,
                TaxAmount = invoice.TaxAmount,
                TotalAmount = invoice.TotalAmount,
                Status = invoice.Status,
                PaymentIds = invoice.PaymentIds,
                CreatedAt = invoice.CreatedAt,
                UpdatedAt = invoice.UpdatedAt
            };
        }
    }
}