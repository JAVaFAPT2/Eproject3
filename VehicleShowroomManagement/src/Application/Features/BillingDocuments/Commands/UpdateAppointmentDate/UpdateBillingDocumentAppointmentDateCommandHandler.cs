namespace VehicleShowroomManagement.Application.Features.BillingDocuments.Commands.UpdateAppointmentDate
{
    /// <summary>
    /// Handler for updating billing document appointment date
    /// </summary>
    public class UpdateBillingDocumentAppointmentDateCommandHandler(IRepository<BillingDocument> billingDocumentRepository) : IRequestHandler<UpdateBillingDocumentAppointmentDateCommand, Unit>
    {

        public async Task<Unit> Handle(UpdateBillingDocumentAppointmentDateCommand request, CancellationToken cancellationToken)
        {
            var billingDocument = await billingDocumentRepository.GetByIdAsync(request.BillingDocumentId, cancellationToken) ?? throw new InvalidOperationException("Billing document not found");

            // Use domain method
            billingDocument.UpdateAppointmentDate(request.AppointmentDate);
            await billingDocumentRepository.UpdateAsync(billingDocument, cancellationToken);

            return Unit.Value;
        }
    }
}

