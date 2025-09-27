using MediatR;

namespace VehicleShowroomManagement.Application.Features.Appointments.Commands.RescheduleAppointment
{
    public class RescheduleAppointmentCommandHandler : IRequestHandler<RescheduleAppointmentCommand>
    {
        public async Task Handle(RescheduleAppointmentCommand request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}