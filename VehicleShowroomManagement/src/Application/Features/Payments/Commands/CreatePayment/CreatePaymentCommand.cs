using MediatR;

namespace VehicleShowroomManagement.Application.Features.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommand : IRequest<string>
    {
        public string InvoiceId { get; set; } = string.Empty;
        public string SalesOrderId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        public string? ReferenceNumber { get; set; }
    }
}