using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Common.Interfaces
{
    /// <summary>
    /// Interface for PDF generation service
    /// </summary>
    public interface IPdfService
    {
        /// <summary>
        /// Generates PDF for order (confirmation/data sheet)
        /// </summary>
        Task<byte[]> GenerateOrderPdfAsync(Order order, User? customer, Vehicle? vehicle, User? dealer);

        /// <summary>
        /// Generates PDF for billing document (invoice)
        /// </summary>
        Task<byte[]> GenerateInvoicePdfAsync(BillingDocument billingDocument, User? customer, Order? order);

        /// <summary>
        /// Generates PDF for purchase order
        /// </summary>
        Task<byte[]> GeneratePurchaseOrderPdfAsync(PurchaseOrder purchaseOrder);
    }
}
