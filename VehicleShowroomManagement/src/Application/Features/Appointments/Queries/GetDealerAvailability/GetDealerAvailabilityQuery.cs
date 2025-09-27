using MediatR;

namespace VehicleShowroomManagement.Application.Features.Appointments.Queries.GetDealerAvailability
{
    public record GetDealerAvailabilityQuery(string DealerId, DateTime FromDate, DateTime ToDate) : IRequest<object>;
}