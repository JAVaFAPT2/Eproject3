namespace VehicleShowroomManagement.Application.Features.BillingDocuments.Commands.UpdateAppointmentDate
{
    /// <summary>
    /// Command to update billing document appointment date
    /// </summary>
    public record UpdateBillingDocumentAppointmentDateCommand(
        string BillingDocumentId,
        DateTime? AppointmentDate)
        : IRequest<Unit>;
}

