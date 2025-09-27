using MediatR;

namespace VehicleShowroomManagement.Application.Features.Allotments.Commands.CreateAllotment
{
    public class CreateAllotmentCommand : IRequest<string>
    {
        public string AllotmentId { get; set; } = string.Empty;
        public string VehicleId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string EmployeeId { get; set; } = string.Empty;
        public DateTime AllotmentDate { get; set; }
    }
}