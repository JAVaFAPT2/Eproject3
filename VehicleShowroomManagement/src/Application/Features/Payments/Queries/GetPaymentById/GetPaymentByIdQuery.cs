using MediatR;

namespace VehicleShowroomManagement.Application.Features.Payments.Queries.GetPaymentById
{
    public class GetPaymentByIdQuery : IRequest<PaymentDto>
    {
        public string PaymentId { get; set; } = string.Empty;
    }
}