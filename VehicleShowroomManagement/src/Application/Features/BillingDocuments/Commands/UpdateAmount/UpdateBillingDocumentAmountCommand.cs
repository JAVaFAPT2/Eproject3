namespace VehicleShowroomManagement.Application.Features.BillingDocuments.Commands.UpdateAmount
{
    /// <summary>
    /// Command to update billing document amount
    /// </summary>
    public record UpdateBillingDocumentAmountCommand(
        string BillingDocumentId,
        decimal Amount)
        : IRequest<Unit>;
}

