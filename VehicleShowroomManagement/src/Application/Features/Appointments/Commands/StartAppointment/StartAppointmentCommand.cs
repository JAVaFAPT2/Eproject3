using MediatR;

namespace VehicleShowroomManagement.Application.Features.Appointments.Commands.StartAppointment
{
    public record StartAppointmentCommand(string Id) : IRequest;
}