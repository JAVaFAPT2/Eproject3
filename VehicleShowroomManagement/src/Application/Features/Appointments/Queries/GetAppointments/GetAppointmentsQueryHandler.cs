using MediatR;

namespace VehicleShowroomManagement.Application.Features.Appointments.Queries.GetAppointments
{
    public class GetAppointmentsQueryHandler : IRequestHandler<GetAppointmentsQuery, object>
    {
        public async Task<object> Handle(GetAppointmentsQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new { message = "Appointments query ready", pageNumber = request.PageNumber };
        }
    }
}