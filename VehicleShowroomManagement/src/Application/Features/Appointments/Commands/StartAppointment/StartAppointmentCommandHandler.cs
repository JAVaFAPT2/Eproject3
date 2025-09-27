using MediatR;

namespace VehicleShowroomManagement.Application.Features.Appointments.Commands.StartAppointment
{
    public class StartAppointmentCommandHandler : IRequestHandler<StartAppointmentCommand>
    {
        public async Task Handle(StartAppointmentCommand request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}