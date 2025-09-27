using MediatR;

namespace VehicleShowroomManagement.Application.Features.Appointments.Commands.CreateAppointment
{
    /// <summary>
    /// Handler for create appointment command
    /// </summary>
    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, string>
    {
        public async Task<string> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            // Simplified implementation - return generated ID
            await Task.CompletedTask;
            return Guid.NewGuid().ToString();
        }
    }
}