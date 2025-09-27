using MediatR;

namespace VehicleShowroomManagement.Application.Features.Appointments.Commands.UpdateAppointmentRequirements
{
    public record UpdateAppointmentRequirementsCommand(string Id, string CustomerRequirements, string? VehicleInterest, string? BudgetRange) : IRequest;
}