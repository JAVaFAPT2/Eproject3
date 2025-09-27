using MediatR;

namespace VehicleShowroomManagement.Application.Features.Allotments.Queries.GetAllotmentById
{
    public class GetAllotmentByIdQuery : IRequest<AllotmentDto>
    {
        public string AllotmentId { get; set; } = string.Empty;
    }
}