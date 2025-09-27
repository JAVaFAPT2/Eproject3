using MediatR;

namespace VehicleShowroomManagement.Application.Features.Appointments.Queries.GetDealerAvailability
{
    public class GetDealerAvailabilityQueryHandler : IRequestHandler<GetDealerAvailabilityQuery, object>
    {
        public async Task<object> Handle(GetDealerAvailabilityQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new { 
                dealerId = request.DealerId, 
                availableSlots = new[] { "09:00", "10:00", "11:00", "14:00", "15:00" },
                message = "Dealer availability ready" 
            };
        }
    }
}