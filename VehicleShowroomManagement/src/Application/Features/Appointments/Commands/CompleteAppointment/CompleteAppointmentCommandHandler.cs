using MediatR;

namespace VehicleShowroomManagement.Application.Features.Appointments.Commands.CompleteAppointment
{
    public class CompleteAppointmentCommandHandler : IRequestHandler<CompleteAppointmentCommand>
    {
        public async Task Handle(CompleteAppointmentCommand request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}