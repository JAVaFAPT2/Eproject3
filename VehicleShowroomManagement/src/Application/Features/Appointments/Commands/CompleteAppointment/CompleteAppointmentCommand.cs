using MediatR;

namespace VehicleShowroomManagement.Application.Features.Appointments.Commands.CompleteAppointment
{
    public record CompleteAppointmentCommand(string Id, string? DealerNotes, bool FollowUpRequired, DateTime? FollowUpDate) : IRequest;
}