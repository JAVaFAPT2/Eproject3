using MediatR;

namespace VehicleShowroomManagement.Application.Features.ReturnRequests.Commands.CreateReturnRequest
{
    public class CreateReturnRequestCommand : IRequest<string>
    {
        public string OrderId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal RefundAmount { get; set; }
    }
}