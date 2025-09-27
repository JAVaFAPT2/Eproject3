using MediatR;

namespace VehicleShowroomManagement.Application.Features.ReturnRequests.Queries.GetReturnRequestById
{
    public class GetReturnRequestByIdQuery : IRequest<ReturnRequestDto>
    {
        public string ReturnRequestId { get; set; } = string.Empty;
    }
}