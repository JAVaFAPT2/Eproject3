using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.Text;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Infrastructure.Services
{
    /// <summary>
    /// Implementation of PDF generation service using iText7
    /// </summary>
    public class PdfService : IPdfService
    {
        public async Task<byte[]> GenerateOrderPdfAsync(SalesOrder order, Customer? customer, Vehicle? vehicle, User? salesPerson)
        {
            var html = GenerateOrderHtml(order, customer, vehicle, salesPerson);
            return await Task.Run(() => ConvertHtmlToPdf(html));
        }

        public async Task<byte[]> GenerateInvoicePdfAsync(Invoice invoice, Customer? customer, SalesOrder? order)
        {
            var html = GenerateInvoiceHtml(invoice, customer, order);
            return await Task.Run(() => ConvertHtmlToPdf(html));
        }

        public async Task<byte[]> GeneratePurchaseOrderPdfAsync(PurchaseOrder purchaseOrder, Supplier? supplier)
        {
            var html = GeneratePurchaseOrderHtml(purchaseOrder, supplier);
            return await Task.Run(() => ConvertHtmlToPdf(html));
        }

        public async Task<byte[]> GenerateGoodsReceiptPdfAsync(GoodsReceipt goodsReceipt, PurchaseOrder? purchaseOrder)
        {
            var html = GenerateGoodsReceiptHtml(goodsReceipt, purchaseOrder);
            return await Task.Run(() => ConvertHtmlToPdf(html));
        }

        public async Task<byte[]> GenerateExcelReportAsync<T>(List<T> data, string worksheetName)
        {
            // This would be implemented with EPPlus or similar library
            // For now, returning empty byte array
            return await Task.FromResult(Array.Empty<byte>());
        }

        private string GenerateOrderHtml(SalesOrder order, Customer? customer, Vehicle? vehicle, User? salesPerson)
        {
            var html = new StringBuilder();
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html><head><style>");
            html.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; }");
            html.AppendLine(".header { text-align: center; margin-bottom: 30px; }");
            html.AppendLine(".section { margin-bottom: 20px; }");
            html.AppendLine(".label { font-weight: bold; }");
            html.AppendLine("table { width: 100%; border-collapse: collapse; }");
            html.AppendLine("th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }");
            html.AppendLine("th { background-color: #f2f2f2; }");
            html.AppendLine("</style></head><body>");
            
            html.AppendLine("<div class='header'>");
            html.AppendLine("<h1>SALES ORDER</h1>");
            html.AppendLine($"<h2>Order #{order.OrderNumber}</h2>");
            html.AppendLine("</div>");

            html.AppendLine("<div class='section'>");
            html.AppendLine("<h3>Order Information</h3>");
            html.AppendLine($"<p><span class='label'>Order Date:</span> {order.OrderDate:yyyy-MM-dd}</p>");
            html.AppendLine($"<p><span class='label'>Status:</span> {order.Status}</p>");
            html.AppendLine($"<p><span class='label'>Payment Method:</span> {order.PaymentMethod}</p>");
            html.AppendLine("</div>");

            if (customer != null)
            {
                html.AppendLine("<div class='section'>");
                html.AppendLine("<h3>Customer Information</h3>");
                html.AppendLine($"<p><span class='label'>Name:</span> {customer.FirstName} {customer.LastName}</p>");
                html.AppendLine($"<p><span class='label'>Email:</span> {customer.Email}</p>");
                html.AppendLine($"<p><span class='label'>Phone:</span> {customer.Phone}</p>");
                html.AppendLine("</div>");
            }

            if (vehicle != null)
            {
                html.AppendLine("<div class='section'>");
                html.AppendLine("<h3>Vehicle Information</h3>");
                html.AppendLine($"<p><span class='label'>Model:</span> {vehicle.ModelNumber}</p>");
            html.AppendLine($"<p><span class='label'>VIN:</span> {vehicle.Vin}</p>");
            html.AppendLine($"<p><span class='label'>Model:</span> {vehicle.ModelNumber}</p>");
            html.AppendLine($"<p><span class='label'>Status:</span> {vehicle.Status}</p>");
                html.AppendLine("</div>");
            }

            html.AppendLine("<div class='section'>");
            html.AppendLine("<h3>Total Amount</h3>");
            html.AppendLine($"<p style='font-size: 24px; font-weight: bold;'>${order.TotalAmount:F2}</p>");
            html.AppendLine("</div>");

            html.AppendLine("</body></html>");
            return html.ToString();
        }

        private string GenerateInvoiceHtml(Invoice invoice, Customer? customer, SalesOrder? order)
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
            html.AppendLine("<h1>INVOICE</h1>");
            html.AppendLine($"<h2>Invoice #{invoice.InvoiceNumber}</h2>");
            html.AppendLine("</div>");

            html.AppendLine("<div class='section'>");
            html.AppendLine($"<p><span class='label'>Invoice Date:</span> {invoice.CreatedAt:yyyy-MM-dd}</p>");
            html.AppendLine($"<p><span class='label'>Invoice Number:</span> {invoice.InvoiceNumber}</p>");
            html.AppendLine($"<p><span class='label'>Total:</span> ${invoice.TotalAmount:F2}</p>");
            html.AppendLine("</div>");

            html.AppendLine("</body></html>");
            return html.ToString();
        }

        private string GeneratePurchaseOrderHtml(PurchaseOrder purchaseOrder, Supplier? supplier)
        {
            var html = new StringBuilder();
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html><head><style>");
            html.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; }");
            html.AppendLine(".header { text-align: center; margin-bottom: 30px; }");
            html.AppendLine("</style></head><body>");
            
            html.AppendLine("<div class='header'>");
            html.AppendLine("<h1>PURCHASE ORDER</h1>");
            html.AppendLine($"<h2>PO #{purchaseOrder.OrderNumber}</h2>");
            html.AppendLine("</div>");

            html.AppendLine("</body></html>");
            return html.ToString();
        }

        private string GenerateGoodsReceiptHtml(GoodsReceipt goodsReceipt, PurchaseOrder? purchaseOrder)
        {
            var html = new StringBuilder();
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html><head><style>");
            html.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; }");
            html.AppendLine(".header { text-align: center; margin-bottom: 30px; }");
            html.AppendLine("</style></head><body>");
            
            html.AppendLine("<div class='header'>");
            html.AppendLine("<h1>GOODS RECEIPT</h1>");
            html.AppendLine($"<h2>Receipt #{goodsReceipt.ReceiptNumber}</h2>");
            html.AppendLine("</div>");

            html.AppendLine("</body></html>");
            return html.ToString();
        }

        private byte[] ConvertHtmlToPdf(string html)
        {
            using var stream = new MemoryStream();
            using var writer = new PdfWriter(stream);
            using var pdf = new PdfDocument(writer);
            using var document = new Document(pdf);

            HtmlConverter.ConvertToPdf(html, stream);
            return stream.ToArray();
        }
    }
}