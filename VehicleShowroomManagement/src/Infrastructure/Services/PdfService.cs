using iText.Html2pdf;
using iText.Kernel.Pdf;
using System.Text;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Application.Common.Exceptions;
using Microsoft.Extensions.Logging;

namespace VehicleShowroomManagement.Infrastructure.Services
{
    /// <summary>
    /// Implementation of PDF generation service using iText7
    /// </summary>
    public class PdfService : BaseService, IPdfService
    {
        public PdfService(ILogger<PdfService> logger) : base(logger)
        {
        }
        public async Task<byte[]> GenerateOrderPdfAsync(Order order, User? customer, Vehicle? vehicle, User? dealer)
        {
            LogOperationStart(nameof(GenerateOrderPdfAsync), new { orderId = order.Id });

            try
            {
                var html = GenerateOrderHtml(order, customer, vehicle, dealer);
                var result = await Task.Run(() => ConvertHtmlToPdf(html));
                
                LogOperationComplete(nameof(GenerateOrderPdfAsync), new { orderId = order.Id, fileSize = result.Length });
                return result;
            }
            catch (Exception ex)
            {
                LogOperationError(nameof(GenerateOrderPdfAsync), ex, new { orderId = order.Id });
                throw new PdfGenerationException($"Failed to generate order PDF: {ex.Message}", ex);
            }
        }

        public async Task<byte[]> GenerateInvoicePdfAsync(BillingDocument billingDocument, User? customer, Order? order)
        {
            LogOperationStart(nameof(GenerateInvoicePdfAsync), new { billingDocumentId = billingDocument.Id });

            try
            {
                var html = GenerateInvoiceHtml(billingDocument, customer, order);
                var result = await Task.Run(() => ConvertHtmlToPdf(html));
                
                LogOperationComplete(nameof(GenerateInvoicePdfAsync), new { billingDocumentId = billingDocument.Id, fileSize = result.Length });
                return result;
            }
            catch (Exception ex)
            {
                LogOperationError(nameof(GenerateInvoicePdfAsync), ex, new { billingDocumentId = billingDocument.Id });
                throw new PdfGenerationException($"Failed to generate invoice PDF: {ex.Message}", ex);
            }
        }

        public async Task<byte[]> GeneratePurchaseOrderPdfAsync(PurchaseOrder purchaseOrder)
        {
            LogOperationStart(nameof(GeneratePurchaseOrderPdfAsync), new { purchaseOrderId = purchaseOrder.Id });

            try
            {
                var html = GeneratePurchaseOrderHtml(purchaseOrder);
                var result = await Task.Run(() => ConvertHtmlToPdf(html));
                
                LogOperationComplete(nameof(GeneratePurchaseOrderPdfAsync), new { purchaseOrderId = purchaseOrder.Id, fileSize = result.Length });
                return result;
            }
            catch (Exception ex)
            {
                LogOperationError(nameof(GeneratePurchaseOrderPdfAsync), ex, new { purchaseOrderId = purchaseOrder.Id });
                throw new PdfGenerationException($"Failed to generate purchase order PDF: {ex.Message}", ex);
            }
        }

        private string GenerateOrderHtml(Order order, User? customer, Vehicle? vehicle, User? dealer)
        {
            var html = new StringBuilder();
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html><head><style>");
            html.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; }");
            html.AppendLine(".header { text-align: center; margin-bottom: 30px; }");
            html.AppendLine(".section { margin-bottom: 20px; }");
            html.AppendLine(".label { font-weight: bold; }");
            html.AppendLine("</style></head><body>");
            
            html.AppendLine("<div class='header'>");
            html.AppendLine("<h1>ORDER CONFIRMATION</h1>");
            html.AppendLine($"<h2>Order #{order.Id}</h2>");
            html.AppendLine("</div>");

            html.AppendLine("<div class='section'>");
            html.AppendLine("<h3>Order Information</h3>");
            html.AppendLine($"<p><span class='label'>Order Date:</span> {order.OrderDate:yyyy-MM-dd}</p>");
            html.AppendLine($"<p><span class='label'>Status:</span> {order.Status}</p>");
            html.AppendLine($"<p><span class='label'>Sale Price:</span> ${order.SalePrice:F2}</p>");
            html.AppendLine("</div>");

            if (customer != null)
            {
                html.AppendLine("<div class='section'>");
                html.AppendLine("<h3>Customer Information</h3>");
                html.AppendLine($"<p><span class='label'>Name:</span> {customer.Name}</p>");
                html.AppendLine($"<p><span class='label'>Email:</span> {customer.Email}</p>");
                html.AppendLine($"<p><span class='label'>Phone:</span> {customer.Phone}</p>");
                html.AppendLine("</div>");
            }

            if (vehicle != null)
            {
                html.AppendLine("<div class='section'>");
                html.AppendLine("<h3>Vehicle Information</h3>");
                html.AppendLine($"<p><span class='label'>Vehicle ID:</span> {vehicle.VehicleId}</p>");
                html.AppendLine($"<p><span class='label'>Model Number:</span> {vehicle.ModelNumber}</p>");
                html.AppendLine($"<p><span class='label'>Status:</span> {vehicle.Status}</p>");
                html.AppendLine("</div>");
            }

            html.AppendLine("</body></html>");
            return html.ToString();
        }

        private string GenerateInvoiceHtml(BillingDocument billingDocument, User? customer, Order? order)
        {
            var html = new StringBuilder();
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html><head><style>");
            html.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; }");
            html.AppendLine(".header { text-align: center; margin-bottom: 30px; }");
            html.AppendLine("</style></head><body>");
            
            html.AppendLine("<div class='header'>");
            html.AppendLine("<h1>INVOICE</h1>");
            html.AppendLine($"<h2>Bill #{billingDocument.Id}</h2>");
            html.AppendLine("</div>");

            html.AppendLine($"<p><span class='label'>Bill Date:</span> {billingDocument.BillDate:yyyy-MM-dd}</p>");
            html.AppendLine($"<p><span class='label'>Amount:</span> ${billingDocument.Amount:F2}</p>");
            html.AppendLine($"<p><span class='label'>Status:</span> {billingDocument.Status}</p>");

            html.AppendLine("</body></html>");
            return html.ToString();
        }

        private string GeneratePurchaseOrderHtml(PurchaseOrder purchaseOrder)
        {
            var html = new StringBuilder();
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html><head><style>");
            html.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; }");
            html.AppendLine(".header { text-align: center; margin-bottom: 30px; }");
            html.AppendLine("</style></head><body>");
            
            html.AppendLine("<div class='header'>");
            html.AppendLine("<h1>PURCHASE ORDER</h1>");
            html.AppendLine($"<h2>PO #{purchaseOrder.Id}</h2>");
            html.AppendLine("</div>");

            html.AppendLine($"<p><span class='label'>Order Date:</span> {purchaseOrder.OrderDate:yyyy-MM-dd}</p>");
            html.AppendLine($"<p><span class='label'>Total Amount:</span> ${purchaseOrder.TotalAmount:F2}</p>");
            html.AppendLine($"<p><span class='label'>Status:</span> {purchaseOrder.Status}</p>");

            html.AppendLine("</body></html>");
            return html.ToString();
        }

        private byte[] ConvertHtmlToPdf(string html)
        {
            using var stream = new MemoryStream();
            HtmlConverter.ConvertToPdf(html, stream);
            return stream.ToArray();
        }
    }
}
