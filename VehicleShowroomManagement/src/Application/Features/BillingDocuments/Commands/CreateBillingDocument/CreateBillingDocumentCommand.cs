using MediatR;

namespace VehicleShowroomManagement.Application.Features.BillingDocuments.Commands.CreateBillingDocument
{
    public class CreateBillingDocumentCommand : IRequest<string>
    {
        public string BillNumber { get; set; } = string.Empty;
        public string SalesOrderId { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public DateTime BillDate { get; set; }
        public decimal Amount { get; set; }
    }
}