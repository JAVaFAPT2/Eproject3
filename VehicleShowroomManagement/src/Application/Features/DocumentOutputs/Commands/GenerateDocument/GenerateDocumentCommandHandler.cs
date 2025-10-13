namespace VehicleShowroomManagement.Application.Features.DocumentOutputs.Commands.GenerateDocument
{
    public class GenerateDocumentCommandHandler(
        IRepository<DocumentOutput> documentOutputRepository,
        IRepository<Order> orderRepository,
        IRepository<BillingDocument> billingDocumentRepository,
        IRepository<PurchaseOrder> purchaseOrderRepository,
        IPdfService pdfService) : IRequestHandler<GenerateDocumentCommand, string>
    {

        public async Task<string> Handle(GenerateDocumentCommand request, CancellationToken cancellationToken)
        {
            byte[] pdfBytes;
            string fileName;

            // Generate PDF based on entity type and file type
            switch (request.EntityType)
            {
                case EntityType.Order:
                    var order = await orderRepository.GetByIdAsync(request.EntityId, cancellationToken) ?? throw new InvalidOperationException("Order not found");
                    pdfBytes = await pdfService.GenerateOrderPdfAsync(order, null, null, null);
                    fileName = $"order-{request.FileType.ToString().ToLower()}-{order.Id}.pdf";
                    break;

                case EntityType.BillingDocument:
                    var billing = await billingDocumentRepository.GetByIdAsync(request.EntityId, cancellationToken) ?? throw new InvalidOperationException("Billing document not found");
                    
                    pdfBytes = await pdfService.GenerateInvoicePdfAsync(billing, null, null);
                    fileName = $"invoice-{billing.Id}.pdf";
                    break;

                case EntityType.PurchaseOrder:
                    var po = await purchaseOrderRepository.GetByIdAsync(request.EntityId, cancellationToken) ?? throw new InvalidOperationException("Purchase order not found");
                    
                    pdfBytes = await pdfService.GeneratePurchaseOrderPdfAsync(po);
                    fileName = $"purchase-order-{po.Id}.pdf";
                    break;

                default:
                    throw new ArgumentException("Invalid entity type");
            }

            // Save to local file system (for production, upload to cloud storage)
            var documentsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents");
            Directory.CreateDirectory(documentsPath);
            var filePath = Path.Combine(documentsPath, fileName);
            await File.WriteAllBytesAsync(filePath, pdfBytes, cancellationToken);
            
            var fileUrl = $"/documents/{fileName}";

            // Create document output record
            var documentOutput = new DocumentOutput(
                request.EntityType,
                request.EntityId,
                request.FileType,
                fileUrl);

            await documentOutputRepository.AddAsync(documentOutput, cancellationToken);

            return documentOutput.Id;
        }
    }
}

