namespace VehicleShowroomManagement.Application.Features.BillingDocuments.Commands.UpdateStatus
{
    /// <summary>
    /// Command to update billing document status
    /// </summary>
    public record UpdateBillingDocumentStatusCommand(
        string BillingDocumentId,
        BillingStatus Status)
        : IRequest<Unit>;
}

