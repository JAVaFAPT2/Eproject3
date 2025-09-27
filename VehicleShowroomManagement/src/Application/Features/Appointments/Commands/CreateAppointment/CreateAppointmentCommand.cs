using MediatR;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Application.Features.Appointments.Commands.CreateAppointment
{
    /// <summary>
    /// Command for creating an appointment
    /// </summary>
    public record CreateAppointmentCommand(
        string CustomerId,
        string DealerId,
        DateTime AppointmentDate,
        AppointmentType Type,
        int DurationMinutes,
        string? CustomerRequirements,
        string? VehicleInterest,
        string? BudgetRange) : IRequest<string>;
}