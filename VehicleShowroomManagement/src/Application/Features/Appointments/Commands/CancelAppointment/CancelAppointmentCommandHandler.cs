using MediatR;

namespace VehicleShowroomManagement.Application.Features.Appointments.Commands.CancelAppointment
{
    public class CancelAppointmentCommandHandler : IRequestHandler<CancelAppointmentCommand>
    {
        public async Task Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}