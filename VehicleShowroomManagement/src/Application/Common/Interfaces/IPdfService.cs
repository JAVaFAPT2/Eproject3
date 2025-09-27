using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Common.Interfaces
{
    /// <summary>
    /// Interface for PDF generation service
    /// </summary>
    public interface IPdfService
    {
        /// <summary>
        /// Generates PDF for sales order
        /// </summary>
        Task<byte[]> GenerateOrderPdfAsync(SalesOrder order, Customer? customer, Vehicle? vehicle, User? salesPerson);

        /// <summary>
        /// Generates PDF for invoice
        /// </summary>
        Task<byte[]> GenerateInvoicePdfAsync(Invoice invoice, Customer? customer, SalesOrder? order);

        /// <summary>
        /// Generates PDF for purchase order
        /// </summary>
        Task<byte[]> GeneratePurchaseOrderPdfAsync(PurchaseOrder purchaseOrder, Supplier? supplier);

        /// <summary>
        /// Generates PDF for goods receipt
        /// </summary>
        Task<byte[]> GenerateGoodsReceiptPdfAsync(GoodsReceipt goodsReceipt, PurchaseOrder? purchaseOrder);

        /// <summary>
        /// Generates Excel report
        /// </summary>
        Task<byte[]> GenerateExcelReportAsync<T>(List<T> data, string worksheetName);
    }
}