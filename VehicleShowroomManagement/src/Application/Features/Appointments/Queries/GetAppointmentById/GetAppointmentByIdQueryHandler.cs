using MediatR;

namespace VehicleShowroomManagement.Application.Features.Appointments.Queries.GetAppointmentById
{
    public class GetAppointmentByIdQueryHandler : IRequestHandler<GetAppointmentByIdQuery, object?>
    {
        public async Task<object?> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            return new { id = request.Id, message = "Appointment details ready" };
        }
    }
}