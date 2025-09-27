using MediatR;

namespace VehicleShowroomManagement.Application.Features.Appointments.Commands.ConfirmAppointment
{
    public record ConfirmAppointmentCommand(string Id, string? DealerNotes) : IRequest;
}