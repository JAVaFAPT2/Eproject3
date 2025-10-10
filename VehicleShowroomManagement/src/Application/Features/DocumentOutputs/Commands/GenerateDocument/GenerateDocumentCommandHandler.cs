using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Application.Features.DocumentOutputs.Commands.GenerateDocument
{
    public class GenerateDocumentCommandHandler : IRequestHandler<GenerateDocumentCommand, string>
    {
        private readonly IRepository<DocumentOutput> _documentOutputRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<BillingDocument> _billingDocumentRepository;
        private readonly IRepository<PurchaseOrder> _purchaseOrderRepository;
        private readonly IPdfService _pdfService;
        private readonly ICloudinaryService _cloudinaryService;

        public GenerateDocumentCommandHandler(
            IRepository<DocumentOutput> documentOutputRepository,
            IRepository<Order> orderRepository,
            IRepository<BillingDocument> billingDocumentRepository,
            IRepository<PurchaseOrder> purchaseOrderRepository,
            IPdfService pdfService,
            ICloudinaryService cloudinaryService)
        {
            _documentOutputRepository = documentOutputRepository;
            _orderRepository = orderRepository;
            _billingDocumentRepository = billingDocumentRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
            _pdfService = pdfService;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<string> Handle(GenerateDocumentCommand request, CancellationToken cancellationToken)
        {
            byte[] pdfBytes;
            string fileName;

            // Generate PDF based on entity type and file type
            switch (request.EntityType)
            {
                case EntityType.Order:
                    var order = await _orderRepository.GetByIdAsync(request.EntityId);
                    if (order == null) throw new InvalidOperationException("Order not found");
                    
                    pdfBytes = await _pdfService.GenerateOrderPdfAsync(order, null, null, null);
                    fileName = $"order-{request.FileType.ToString().ToLower()}-{order.Id}.pdf";
                    break;

                case EntityType.BillingDocument:
                    var billing = await _billingDocumentRepository.GetByIdAsync(request.EntityId);
                    if (billing == null) throw new InvalidOperationException("Billing document not found");
                    
                    pdfBytes = await _pdfService.GenerateInvoicePdfAsync(billing, null, null);
                    fileName = $"invoice-{billing.Id}.pdf";
                    break;

                case EntityType.PurchaseOrder:
                    var po = await _purchaseOrderRepository.GetByIdAsync(request.EntityId);
                    if (po == null) throw new InvalidOperationException("Purchase order not found");
                    
                    pdfBytes = await _pdfService.GeneratePurchaseOrderPdfAsync(po);
                    fileName = $"purchase-order-{po.Id}.pdf";
                    break;

                default:
                    throw new ArgumentException("Invalid entity type");
            }

            // Save to local file system (for production, upload to cloud storage)
            var documentsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documents");
            Directory.CreateDirectory(documentsPath);
            var filePath = Path.Combine(documentsPath, fileName);
            await File.WriteAllBytesAsync(filePath, pdfBytes);
            
            var fileUrl = $"/documents/{fileName}";

            // Create document output record
            var documentOutput = new DocumentOutput(
                request.EntityType,
                request.EntityId,
                request.FileType,
                fileUrl);

            await _documentOutputRepository.AddAsync(documentOutput);

            return documentOutput.Id;
        }
    }
}

