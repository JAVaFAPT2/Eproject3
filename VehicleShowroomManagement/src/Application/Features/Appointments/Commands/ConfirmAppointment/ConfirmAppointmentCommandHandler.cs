using MediatR;

namespace VehicleShowroomManagement.Application.Features.Appointments.Commands.ConfirmAppointment
{
    public class ConfirmAppointmentCommandHandler : IRequestHandler<ConfirmAppointmentCommand>
    {
        public async Task Handle(ConfirmAppointmentCommand request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}